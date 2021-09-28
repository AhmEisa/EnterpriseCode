using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enterprise.Web.CustomAuthorization;
using Enterprise.Web.Data;
using Enterprise.Web.Middleware;
using Enterprise.Web.Validations;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;

namespace Enterprise.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //ControllerAction : EndpointDataSource=>CreateEndpoints(): create an endpoint for each action method in controllers and store them in
            // Route Collection property called Endpoints.

            services.AddRazorPages();

            // تحويل كل الريكوستات الى استخدام https لحماية البيانات المرسلة فى الريكوست وذلك فى أى بيئة غير بيئة التطوير
            // يمكن استخدام القيمة [RequireHttps] على مستوى الكنترولر او الاكشن
            if (!_env.IsDevelopment())
                services.Configure<MvcOptions>(o => o.Filters.Add(new RequireHttpsAttribute()));

            // لمنع نداء صفحات الموقع من مواقع أخرى عن طريق جافا سكربت ريكوست أجاكس نستخدم الوظيفة 
            //Cross-Origin-Resource-Sharing(CORS)
            //Access-Control-Allow-Origin: */http://bank.com
            //Preflight Request Skip Conditions: 
            services.AddCors(opts =>
            {
                opts.AddPolicy("PloicyName", c => c.WithOrigins("https://bank.com").WithMethods("GET"));
            });

            //يستخدم التشفير المتماثل فى التطبيق مع استخدام التشفير ذو الطريق الواحد فى التشفير 
            //Use Encryption with Hasing
            //old technology use machine.config file for attaching key but that is hard to sync in web farm scenarios and no key rotation or key protection.
            // USe Data Protection Api as it replacement for machine config and provide key rotation and protected and keys are per purpose and per applications
            //IDataProtectionProvider=>IDataProtector=>encrypted string
            // master key + purpose key = used key
            //master keys saved in : User Profile available(local app data folder + DPAPI) - IIS(Registry+DPAPI) - Azure(Data-Protection-Keys folder)
            services.AddDataProtection();
            services.AddSingleton<PurposeStringConstants>();

            //لتخزين البيانات المهمة فى الاعدادات يمكن استخدام اكتر من اعداد حسب بيئة العمل مع تعريف القيم المتغيره فى كل بيئة عمل
            //use Environment Variables

            //Identity framework
            //Claims-based Identity
            //ClaimsPrincipal(User)=>set of ClaimsIdentity => each of it contains a set of Claims
            //Claims set in Identity Cookie encypted by data protection API and then sent to the browser for subsequent requests.
            //User=> set of UserClaims and set of Roles => ech role can has a set of RoleClaims
            //for more info visit: github.com/aspnet/identity

            services.AddDbContext<IdentityDbContext>();
            services.AddIdentity<IdentityUser, IdentityRole>(opts =>
            {
                opts.Password.RequireNonAlphanumeric = true;

            })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

            //using Json Web Token- JWT
            //Open-Id connect mandates the use of JWT.
            //security tokens are (Protected) data structure contain information about issuer and subject (claims),signed(tamper proof,authenticity),
            //contain expiration time.
            //Client request token => Issuer issues a token => Resource consumes a token (with trust relationship with the issuer ).
            //History:SAML 1.1/2.0 xml based ver expensive - Simple Wek Token(SWT):form/url encoded symmetric signatures only.
            //Json Web Token:easy to create/transmit/parse/validate json encoded - sysmmetric and asymmetric signatures(HMAC SHA256-384,ECDSA,RSA),symmetric and asymmetric encryption(RSA,AES/CGM) 
            //Contents: Header(metadata,algorithms,keys used) base64stirng,
            //          Claims(issuer-iss,audience-aud,issued at-iat,expiration-exp,subject-sub,..and application defined claims) base64string,
            //          Signature of the token hashed using the algorithm. ex:AXS45.FDWERer.GFSSDFDF
            //see HandleJWT class for creating and consuming the token

            //OAuth2: open protocol to allow secure authorization not authenticaion in simple and standard method from web,mobile and desktop applications.
            //makes it HTTP/Json freindly to request and transmit tokens typically for delegated authorizaion(access token).
            //Resource Owner => owns a resource on => Resource Server 
            //uses Confidential/Public trusted/untrusted Client to => register on Authorization Server that trusted by the Resource Server
            // then can access the Resource Server using Token issued from Authorization Server.
            //OAuth2 Flows- with User Interaction:
            //Authorization Code Flow : web application clients- request authoriztion,request token,access resource.
            //Designed for server-based applications that client can store secret securely on the server and access token never leaked to the browser.
            // with long-lived access can be implemented. 
            //GET /authorize?client_id=webApp&scope=resource&redirect_url=http://webapp/callback &response_type=code&state=123
            //GET http://webapp/callback?code=xyz&state=123 => on web app client
            //POST /Token Authorization:Basic (cleint_id:secrect) grant_type=authorization_code&authorization_code=xyz&redirect_uri=https://webapp/cb 
            //then it return short-lived token called access token expired on specific period and long-lived token called refresh-token.
            //Get /resource Authorization: Bearer access_token.
            // after validation of issuer,signature and expiration the client&resouce owner can use the resource
            //Claims for access token include: resource owner identifier,client identifier,granted scope,anything make sense to the application.
            //To refresh the token : POST /Token Authorization Basic(cleint_id:secret) grant_type=refresh_token&refresh_token=xyz
            //basic authentication style: Base64String(client_Id+":"+client_secret) or Oauth Style : Base64String(urlformencoded(client_Id)+":"+urlformencoded(client_secret))

            //Implicit Flow: native/local clients- request authoriztion/token,access resource.
            //GET /authorize?client_id=nativeApp&scope=resource&redirect_uri=http://webapp/cb &response_type=token&state=123
            //GET /cb#access_token=abc&token_type=bearer&expires_in=3600&state=123 => on native app client
            //then it return short-lived token called access token expired on specific period and no refresh-token.
            //Get /resource Authorization: Bearer access_token.

            //OAuth2 Flows- with No User Interaction:
            //Resource Owner Password Credential Flow: trusted clients-request token with resource owner credentials, access resource
            //POST /Token Authorization:Basic (cleint_id:secrect) grant_type=password&scope=resource&user_name=owner&password=password 
            //then it return short-lived token called access token expired on specific period and long-lived token called refresh-token.
            //Get /resource Authorization: Bearer access_token.

            //Client Credential Flow:client to service communication-request token with client credentials,access resource
            //POST /Token Authorization:Basic (cleint_id:secrect) grant_type=client_credentials&scope=resource 

            //Open-id Connect
            //Identify user in application,control access to application features.
            //builds on top of OAuth2 using authorization code flow and implicit flow add new concepts:Id token,userinfo endpoint and add some protocols for
            //discovery&dynamic registration ,session management
            //use log into identity provider using username and password then,interact with authorization endpoint
            //GET /authorize?client_id=webApp&redirect_uri=http://webapp/cb &scope=openid profile/email/address/phone/offline_access&response_type=code&state=123
            //GET /cb?code=abc&state=123 => on web app client
            //POST /Token Authorization:Basic (cleint_id:secrect) grant_type=authorization_code&authorization_code=abc&redirect_uri=https://webapp/cb 
            //then it return short-lived token called access token expired on specific period and long-lived token called refresh-token.
            // then the client must validate the id token (issuer,audience,subject,expiration)
            //Get /userinfo Authorization: Bearer access_token. using userinfo endpoint
            //Get /.well-known/openid-configuration ,/.well-known/jwks  Authorization: Bearer access_token. for list of endpoints and configurations and singning keys.

            //Access token intended audience is the protected resource not the client.

            this.ConfigureAspnetIdentity(services);

            this.ConfigureValidation(services);
        }

        private void ConfigureValidation(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(opts => opts.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>());
        }

        private void ConfigureAspnetIdentity(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(opts => opts.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>();
        }
        private void ConfigureAuthenticationAndAuthorization(IServiceCollection services)
        {
            services.AddAuthentication(opts =>
            {
                opts.AddScheme<AuthHandler>("qsv", "QueryStringValue");
                opts.DefaultScheme = "qsv";
            });

            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(opts =>
            {
                opts.LoginPath = "/signin";
                opts.AccessDeniedPath = "/signin/403";
            });

            services.AddAuthorization();
        }

        private void AddIdentityConfigurations(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("main")));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 2;
            }).AddEntityFrameworkStores<IdentityDbContext>();

            //claims based
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Add", policy => policy.RequireClaim("Add User", "Add User"));
                options.AddPolicy("Edit", policy => policy.RequireClaim("Edit User", "Edit User"));
                options.AddPolicy("MinOrderAge", policy => policy.Requirements.Add(new MinimumOrderAgeRequirement(18)));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // لمعالجة مشكلة أن أول ريكوست يكون غير أمن أى غير مستخدم https وإجبار المتصفح على استخدام التصفح الامن يمكن عمل ذلك عن طريق نيو جت باكدج اسمها NWebsec.AspNetCore.Middleware
            //Strict-Transport-Security:max-age=31536000; header
            //يجب تسجيل الدومين فى هذا الموقع لكى يتعرف المتصفح على أنه لابد من استخدام التصفح الامن  https://hstspreload.org/
            app.UseHsts(cnfg => cnfg.MaxAge(days: 365).Preload());

            //SQL-Injection
            // لمنع انتهاء جملة السيكوال : يجب فحص المدخلات على انها لاء تحتوى على كوت او سلاش كما يجب اعطاء اقل صلاحية للهاكر فى حالة الاختراق أو يجب استخدام ORM كما يجب عدم استخدام الستورد بروسيدور

            // Cross-Site-Request-Forgery(CSRF)
            // يجب استخدام وظيفة إضافة الكوكيز لأى نموذج إدخال بيانات لمنع انتهاك اى صلاحية للمستخدم وذلك باستخدام [ValidateAntiForgeryToken] على مستوى الاكشن Post

            //Cross-Site Scripting(XSS)
            //محاولة إدخال بعض البيانات على طريقة سكربتات لتنفيذها عند العرض فى المتصفح لذلك يجب تحويل هذه البيانات الى الطريقة الامنه قبل العرض او منعها فى الادخال ان امكن ذلك
            //mvc encodes everything that is in a variable automatically=> encode and decode => html.encode

            //لاتاحة خاصية التحقق من السكربتات فى الموقع واضافة الهيدر ومنع تحميل اى سكربتات من خارج الموقع
            //Content-Security-Policy: script-src 'self' default-src for all [script,style,img,media,frame,font]
            app.UseCsp(opts => opts.DefaultSources(s => s.Self())
            .StyleSources(s => s.Self().CustomSources("maxcdn.")) // لاتاحة بعض المواقع لتحميل ملفات الاستيلات
            .ScriptSources(s => s.Self().CustomSources("maxcdn.")) // لاتاحة بعض المواقع لتحميل ملفات الاسكربتات
                                                                   // .ReportUris(r => r.Uris("/report")) // لتسجل الصفحات الى تم اضافتها عن طريق مواقع اخرى فى صفحة الريبورتات
            );

            // لتسجل الصفحات الى تم اضافتها عن طريق مواقع اخرى فى صفحة الريبورتات
            // app.UseCspReportOnly(cnfg=>cnfg.ReportUris(c=>c.Uris("/report")));

            // منع محاولة التحول الى موقع أخر عند تسجل الدخول لموقع والتحول لموقع أخر
            //if (!Url.IsLocalUrl(redirectUrl)) فى الاكشن فى الكنترولر
            //Open Redirection Attack

            // لمنع استخدام صلاحيات المصتخدم فى مواقع أخرى عن طريق الضغط على اعلان انك كسبت فلوس 
            //X-Frame-Options: Deny/SAMEORIGIN/ALLOW-FROM https://example.com
            app.UseXfo(x => x.Deny());

            // لمنع نداء صفحات الموقع من مواقع أخرى عن طريق جافا سكربت ريكوست أجاكس نستخدم الوظيفة 
            //Cross-Origin-Resource-Sharing(CORS)
            //Access-Control-Allow-Origin: */http://bank.com
            //Preflight Request Skip Conditions: 
            app.UseCors(cnfg => cnfg.AllowAnyOrigin()); // لاضافة الخاصية على التطبيق ككل 
                                                        //يمكن أضافته على الكنترولر الخاص بالاى بى أى باستخدام [UseCors("PlicyName")]

            //يستخدم التشفير المتماثل فى التطبيق مع استخدام التشفير ذو الطريق الواحد فى التشفير 
            //Use Encryption with Hasing
            //old technology use machine.config file for attaching key but that is hard to sync in web farm scenarios and no key rotation or key protection.
            // USe Data Protection Api as it replacement for machine config and provide key rotation and protected and keys are per purpose and per applications
            //IDataProtectionProvider=>IDataProtector=>encrypted string
            // master key + purpose key = used key
            //master keys saved in : User Profile available(local app data folder + DPAPI) - IIS(Registry+DPAPI) - Azure(Data-Protection-Keys folder)

            //Routing: the process of mapping an incoming request to an endpoint like mvc action method or another one.
            //Types: Conventional Routing:uses wide patterns to match a URL to controller action method.Attribute Routing:attribute applied directly to controller or action.
            //Route Templates are matched based on path segments in the URL.
            //Routing evaluation order : if an endpoint has attributes evaluate them in parallel,otherwise,evaluate any conventional routes in order. 
            // In Legacy: route middleware send the request to MVC Route Handler to select action method which send the response back down middleware pipeline.
            // register endpoints and routes

            app.UseAuthentication();

            app.UseRouting(); // select/decides which endpoint should handle the request

            // add custom middleware between routing selection and execution
            app.UseMiddleware<FeatureSwitchMiddleware>();



            app.UseAuthorization();

            // execute the selected endpoint to generate a response for the request.
            //endpoints: classes the contain a Request Delegate and other metadata used to generate a response.

            app.UseEndpoints(endpoints =>
            {
                //Conventional Routing:uses wide patterns to match a URL to controller action method
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}{id?}");

                // register endpoints for each razor page in the application
                endpoints.MapRazorPages();
                endpoints.MapGet("/secret", SecretEndpoint.Endpoint).WithDisplayName("secret");
                endpoints.Map("/signin", UsersAndClaims.SignIn).WithDisplayName("signin");
                endpoints.Map("/signout", UsersAndClaims.SignOut).WithDisplayName("signout");
            });
        }
    }
}
