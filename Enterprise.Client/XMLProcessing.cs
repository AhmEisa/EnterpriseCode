using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Enterprise.Client
{
    public class XMLProcessing
    {
        private Customer _customer = new Customer { Id = 1, FirstName = "Test", LastName = "Test" };
        private string _resultText = string.Empty;
        private string _xmlFileName = "C:\\XML\\CachedResult.xml";
        public void WriteDataToXML()
        {
            var serailzer = new XmlSerializer(typeof(Customer));
            using (var sw = new StringWriter())
            {
                serailzer.Serialize(sw, _customer);
                _resultText = sw.ToString();
            }
            WriteXMLToFile(_resultText);
        }

        public void WriteXMLToFile(string xml)
        {
            if (!Directory.Exists(Path.GetDirectoryName(_xmlFileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_xmlFileName));
            }

            if (File.Exists(_xmlFileName))
            {
                File.Delete(_xmlFileName);
            }
            File.AppendAllText(_xmlFileName, xml, Encoding.Unicode);
        }

        public void ReadDataFromXML()
        {
            _customer = new Customer();
            var serailzer = new XmlSerializer(typeof(Customer));
            using (var tw = new FileStream(_xmlFileName, FileMode.Open))
            {
                _customer = (Customer)serailzer.Deserialize(tw);
                tw.Close();
            }
            _resultText = string.Empty;
        }

        public void WriteDataToXMLUsingDataContractSerailizer()
        {
            _resultText = string.Empty;
            using (var ms = new MemoryStream())
            {
                DataContractSerializer serailzer = new DataContractSerializer(typeof(Customer));
                serailzer.WriteObject(ms, _customer);

                if (!Directory.Exists(Path.GetDirectoryName(_xmlFileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_xmlFileName));
                }

                if (File.Exists(_xmlFileName))
                {
                    File.Delete(_xmlFileName);
                }

                ms.Seek(0, SeekOrigin.Begin);

                using (FileStream fs = new FileStream(_xmlFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    ms.CopyTo(fs);
                }
            }
        }

        public void ReadDataFromXMLUsingDataContractSerailizer()
        {
            _resultText = string.Empty;
            if (File.Exists(_xmlFileName))
            {
                DataContractSerializer serailzer = new DataContractSerializer(typeof(Customer));
                using (var ms = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(_xmlFileName, FileMode.Open, FileAccess.Read))
                    {
                        fs.CopyTo(ms);
                    }
                    ms.Seek(0, SeekOrigin.Begin);

                    _customer = (Customer)serailzer.ReadObject(ms);
                }
            }
        }

        public void GetData()
        {
            GetLocalInfo();
            GetServerInfo();

            if (true) //if (localMaxDate < serverMaxDate || localTotalRows != ServerTotalRows)
            {
                //GetFromServer();
                //StoreToLocalFile();
                //IsFromLocal = false;
            }
            else
            {
                //GetFromXmlFile();
                //IsFromLocal = true;
            }
        }
        public void GetLocalInfo()
        {
            XElement xElement;
            var LocalMaxDate = DateTime.MinValue;
            var localTotalRows = -1;

            if (File.Exists(_xmlFileName))
            {
                try
                {
                    xElement = XElement.Parse(File.ReadAllText(_xmlFileName));
                    LocalMaxDate = xElement.GetMaxValue<DateTime>("ModifiedDate", "Customer");
                    localTotalRows = xElement.GetCount("Customer");
                }
                catch (Exception ex)
                {
                    _resultText = ex.ToString();
                }
            }
        }

        public void GetServerInfo() { }
    }

    /// <summary>
    /// Use Attributes to override the result
    /// </summary>
    [XmlRoot("CustomerClass", Namespace = "http://www.moj.com")]
    public class Customer
    {
        [XmlAttribute("CustId")]
        public int Id { get; set; }

        [XmlElement("FName")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [XmlArray("OrdersForCustomer")]
        public List<Order> Orders { get; set; } = new List<Order>();
    }

    [DataContract(Name = "Order", Namespace = "http://www.moj.com")]
    public class Order
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }

    public static class XMLSerailzationHelper
    {
        public static string Serialize<T>(this T value)
        {
            string ret = string.Empty;
            if (value != null)
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var sw = new StringWriter())
                {
                    serializer.Serialize(sw, value);
                    ret = sw.ToString();
                }
            }
            return ret;
        }
        public static T Deserialize<T>(this T value, string xml)
        {
            T ret = default(T);

            if (!string.IsNullOrWhiteSpace(xml))
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(xml)))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    ret = (T)serializer.Deserialize(ms);
                }
            }
            return ret;
        }

        public static T GetMaxValue<T>(this XElement elem, string elemName, string parentNodeName)
        {
            T maxValue = default(T); //(from node in elem.Elements(parentNodeName)
            //              select node.Element(elemName).GetAs<T>(default(T))).Max();
            return maxValue;
        }

        public static int GetCount(this XElement elem, string parentNodeName)
        {
            int count = (from node in elem.Elements(parentNodeName)
                         select node).Count();
            return count;
        }
    }
}
