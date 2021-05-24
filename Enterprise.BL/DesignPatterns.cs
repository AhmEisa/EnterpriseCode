using System;
using System.Collections.Generic;
using System.Text;

namespace Enterprise.BL.DesignPatterns.ChainOfResponsibility
{
    public interface IHandler<T> where T : class
    {
        IHandler<T> SetNext(IHandler<T> next);
        void Handle(T request);
    }
    public abstract class Handler<T> : IHandler<T> where T : class
    {
        private IHandler<T> Next { get; set; }
        public virtual void Handle(T request)
        {
            Next?.Handle(request);
        }

        public IHandler<T> SetNext(IHandler<T> next)
        {
            Next = next;
            return Next;
        }
    }
    public class User
    {
        public string SSN { get; set; }
        public string Email { get; set; }
    }
    class SocialSecurityNumberValidationHandler : Handler<User>
    {
        public override void Handle(User request)
        {
            if (string.IsNullOrWhiteSpace(request.SSN)) { throw new ArgumentNullException(paramName: $"{request.SSN}", message: "Socail Security Number can not be Empty."); }
            base.Handle(request);
        }
    }
    class EmailValidationHandler : Handler<User>
    {
        public override void Handle(User request)
        {
            if (string.IsNullOrWhiteSpace(request.SSN)) { throw new ArgumentNullException(paramName: $"{request.Email}", message: "Email can not be Empty."); }
            base.Handle(request);
        }
    }

    public class TryChainOfResponsibility
    {
        public void Run()
        {
            try
            {
                var handler = new SocialSecurityNumberValidationHandler();
                handler.SetNext(new EmailValidationHandler());
                handler.Handle(new User { });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    /*Another implementation */
    public interface IReceiver<T> where T : class
    {
        void Handle(T request);
    }
    public class ServiceRequestValidationHanlder
    {
        private readonly IList<IReceiver<User>> receivers;
        public ServiceRequestValidationHanlder(params IReceiver<User>[] receivers)
        {
            this.receivers = receivers;
        }
        public void Handle(User user)
        {
            foreach (var receiver in receivers)
            {
                receiver.Handle(user);
                // do other logic
            }
        }
        public void SetNext(IReceiver<User> receiver) { this.receivers.Add(receiver); }
    }

}


