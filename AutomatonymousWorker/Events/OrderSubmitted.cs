using System;

namespace AutomatonymousWorker.Events
{
    public interface OrderSubmitted
    {
        Guid OrderId { get; }    
        DateTime OrderDate { get; }
    }
}