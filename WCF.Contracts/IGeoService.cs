using System.Collections.Generic;
using System.ServiceModel;

namespace WCF.Contracts
{
    [ServiceContract]
    public interface IGeoService
    {
        [OperationContract]
        ZipCodeData GetZipInfo(string zip);

        [OperationContract]
        IEnumerable<string> GetStates(bool primaryOnly);

        [OperationContract(Name = "GetZipsByState")]
        IEnumerable<ZipCodeData> GetZips(string state);

        [OperationContract(Name = "GetZipsFromRange")]
        IEnumerable<ZipCodeData> GetZips(string zip, int range);
    }
}
