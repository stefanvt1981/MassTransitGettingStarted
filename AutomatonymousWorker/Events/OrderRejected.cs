using System;

namespace AutomatonymousWorker.Events
{
    public interface OrderRejected
    {
        Guid OrderId { get; } 
        string RejectReason { get; }
    }
}