using System;

namespace AutomatonymousWorker.Events
{
    public interface SubmitOrder
    {
        Guid OrderId { get; }    
        DateTime OrderDate { get; }
    }
}