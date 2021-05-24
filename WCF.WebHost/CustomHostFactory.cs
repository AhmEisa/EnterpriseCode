using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;

namespace WCF.WebHost
{
    public class CustomHostFactory : ServiceHostFactory
    {
        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost host = new ServiceHost(serviceType, baseAddresses);
            // add config here
            return host;
        }
    }
}