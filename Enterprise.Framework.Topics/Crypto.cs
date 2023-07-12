using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Framework.Topics
{
    internal class HashFunctions
    {
        public string FindPreImage(string H)
        {
            //repeat{
            //M= random_message();
            //if Hash(M)==H then return M
            //}

            return string.Empty;
        }

        public string FindSecondPreImage(string M)
        {
            /*
                H = Hash(M)
                return FindPreImage(H)
             */
            return string.Empty;
        }

        /*
         solve-collision() {
            M = random_message()
            return (M, solve-second-preimage(M))
         }

         */

    }
}
