using System;
using System.Collections.Generic;
using System.Text;

namespace Enterprise.Client
{
    // Delegate opeartate as a pipeline between the event raiser and the event handler
    // data can be send across that delegate using EventArgs 
    // Events are notifications to trigger that a thing happened from end users or from objects
    // events based on multicast delegate class that store all the invocation list of listener to that event
    // event handler is responsible for receiving and processing data from a delegate and normally receive two parameters the sender and eventArgs that encpasulate data

    class EventDelegateHandler
    {
        public delegate void WorkPerformedHandler(int hours, string workType);

        static void WorkPerformed1(int hours, string workType) { Console.WriteLine("WorkPerformed1 called"); }
        static void WorkPerformed2(int hours, string workType) { Console.WriteLine("WorkPerformed1 called"); }
        public void InstanciateDelegate()
        {
            WorkPerformedHandler del1 = new WorkPerformedHandler(WorkPerformed1);
            del1(5, "Golf"); // invoke the delegate
            
        }

        public void AddingToTheInvocationList()
        {
            WorkPerformedHandler del1 = new WorkPerformedHandler(WorkPerformed1);
            WorkPerformedHandler del2 = new WorkPerformedHandler(WorkPerformed2);
            del1 += del2;
            del1(5, "Golf"); // invoke the delegate invoke the two delegates handler
        }


    }
}
