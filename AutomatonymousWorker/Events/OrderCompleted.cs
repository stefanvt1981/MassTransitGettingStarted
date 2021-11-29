using System;

namespace AutomatonymousWorker.Events
{
    public interface OrderCompleted
    {
        Guid OrderId { get; }  
    }
}