using System;

namespace AutomatonymousWorker.Events
{
    public interface OrderCancelled
    {
        Guid OrderId { get; }  
    }
}