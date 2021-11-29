using System;

namespace AutomatonymousWorker.Events
{
    public interface OrderAccepted
    {
        Guid OrderId { get; }  
    }
}