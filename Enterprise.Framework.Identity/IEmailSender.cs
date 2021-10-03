using System;

namespace Enterprise.Framework.Identity
{
    public interface IEmailSender
    {
        //https://www.twilio.com/
        public void SendMessage(AppUser user, string subject, params string[] body);
    }
    public class ConsoleEmailSender : IEmailSender
    {
        public void SendMessage(AppUser user, string subject, params string[] body)
        {
            Console.WriteLine("--- Email Starts ---");
            Console.WriteLine($"To: {user.EmailAddress}");
            Console.WriteLine($"Subject: {subject}");
            foreach (string str in body)
            {
                Console.WriteLine(str);
            }
            Console.WriteLine("--- Email Ends ---");
        }
    }
}
