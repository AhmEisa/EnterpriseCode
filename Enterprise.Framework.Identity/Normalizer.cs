using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Framework.Identity
{
    public class Normalizer : ILookupNormalizer
    {
        public string NormalizeEmail(string email) => email.Normalize().ToLowerInvariant();

        public string NormalizeName(string name) => name.Normalize().ToLowerInvariant();
    }
}
