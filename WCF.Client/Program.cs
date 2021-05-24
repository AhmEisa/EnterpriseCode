using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCF.Contracts;
using WCF.Proxies;

namespace WCF.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string title = $"UI Runing on Thread {Thread.CurrentThread.ManagedThreadId} | Process {Process.GetCurrentProcess().Id.ToString()}";

            Console.WriteLine(title);

            Program client = new Program();

            client.AccessTheFirstService("123");

            Console.ReadLine();
        }

        public void AccessTheFirstService(string txtZipCode)
        {
            if (!string.IsNullOrWhiteSpace(txtZipCode))
            {
                GeoClient proxy = new GeoClient("tcpEP");

                ZipCodeData data = proxy.GetZipInfo(txtZipCode);

                if (data != null)
                {
                    // populate the data with controls
                }

                proxy.Close();
            }
        }

        public void AccessTheNextService(string txtState)
        {
            if (!string.IsNullOrWhiteSpace(txtState))
            {
                EndpointAddress address = new EndpointAddress("net.tcp://localhost:8009/GeoService");
                Binding binding = new NetTcpBinding();

                GeoClient proxy = new GeoClient(binding, address);

                IEnumerable<ZipCodeData> data = proxy.GetZips(txtState);

                if (data != null)
                {
                    // populate the data with controls
                }

                proxy.Close();
            }
        }


    }
}
