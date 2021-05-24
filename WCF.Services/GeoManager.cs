using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCF.Contracts;

namespace WCF.Services
{
    public class GeoManager : IGeoService
    {
        public IEnumerable<string> GetStates(bool primaryOnly)
        {
            throw new NotImplementedException();
        }

        public ZipCodeData GetZipInfo(string zip)
        {
            ZipCodeData zipCodeData = null;
            // get data from repository
            //map entity data to data contract 
            return zipCodeData;
        }

        public IEnumerable<ZipCodeData> GetZips(string state)
        {
            IEnumerable<ZipCodeData> codeDatas = new List<ZipCodeData>();
            // get data from repository
            //map entity data to data contract 
            return codeDatas;
        }

        public IEnumerable<ZipCodeData> GetZips(string zip, int range)
        {
            IEnumerable<ZipCodeData> codeDatas = new List<ZipCodeData>();
            // get data from repository
            //map entity data to data contract 
            return codeDatas;
        }
    }
}
