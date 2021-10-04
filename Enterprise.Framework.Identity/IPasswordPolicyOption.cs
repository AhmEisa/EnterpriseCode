using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Enterprise.Framework.Identity
{
    /*
        (?=.*[a-z])         // use positive look ahead to see if at least one lower case letter exists
        (?=.*[A-Z])        // use positive look ahead to see if at least one upper case letter exists
        (?=.*\d)          // use positive look ahead to see if at least one digit exists
        (?=.*\W]) requireNonAlphaNumeric       // use positive look ahead to see if at least one non-word character exists
     */
    public interface IPasswordPolicyOption
    {
        (bool IsValid, string ErrorMessage) IsSatisfied(string password);
    }
    public class PasswordRequiredLengthPolicyOption : IPasswordPolicyOption
    {
        private readonly int requiredLength;

        public PasswordRequiredLengthPolicyOption(int requiredLength)
        {
            this.requiredLength = requiredLength;
        }
        public (bool IsValid, string ErrorMessage) IsSatisfied(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= this.requiredLength ?
                    (true, string.Empty) : (false, $"Passwords must have at least {this.requiredLength} characters.");
        }
    }
    public class PasswordRequireDigitPolicyOption : IPasswordPolicyOption
    {
        public (bool IsValid, string ErrorMessage) IsSatisfied(string password)
        {
            return Regex.Match(password, "(?=.*\\d)").Success ? (true, string.Empty) : (false, $"Passwords must have at least one digit ('0'-'9').");
        }
    }
}
