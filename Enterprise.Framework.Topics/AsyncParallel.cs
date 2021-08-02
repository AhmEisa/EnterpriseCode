using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Framework.Topics
{
    // Task : from the Task Parallel Library : represents a single asynchronous operation
    // used for execute work on a different thread 
    // get the result from the asynchronous operation
    //subscribe when the opreation is done by introducing continuation
    // It can tell if there is execption

    //Task.Run() : queuw work on thread pool to be excuted on different thread
    // Dispatcher.Invoke() to queue work on the UI Thread

    //TaskContinuationOptions : specify the behavior for the task that is created by using the ContinueWith
    // and that creates a task and run asynchrounously

    //Allow users to cancel an operation that waiting for and it is incorrect

    //CancellaionTokenSource : signals to a CancelationToken that it should be cancelled
    //calling cancel will not automatically terminate the async operation


}
