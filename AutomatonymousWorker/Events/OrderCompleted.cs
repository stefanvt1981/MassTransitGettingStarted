using System;

namespace AutomatonymousWorker.Events
{
    public interface FinalizeOrder
    {
        Guid OrderId { get; }  
    }
}