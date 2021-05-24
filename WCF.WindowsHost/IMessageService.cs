using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCF.WindowsHost
{
    [ServiceContract]
    public interface IMessageService
    {
        [OperationContract]
        void ShowMessage(string message);
    }
}
