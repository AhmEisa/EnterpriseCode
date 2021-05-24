using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Enterprise.Client
{
    public class TypedClient
    {
        private readonly HttpClient Client;
        public TypedClient(HttpClient client)
        {
            Client = client;
            Client.BaseAddress = new Uri("server domain or ip");
            Client.Timeout = new TimeSpan(0, 0, 30);
            Client.DefaultRequestHeaders.Clear();
        }

        public async Task Get(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "endpoint name");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response = await this.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();
                var dataReturned = stream.ReadAndSerrializeFromJson<List<object>();
            }
        }
    }
}
