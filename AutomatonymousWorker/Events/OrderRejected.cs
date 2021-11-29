using System;

namespace AutomatonymousWorker.Events
{
    public interface RejectOrder
    {
        Guid OrderId { get; } 
        string Reason { get; }
    }
}