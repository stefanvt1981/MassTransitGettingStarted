using System;

namespace AutomatonymousWorker.Events
{
    public interface AcceptOrder
    {
        Guid OrderId { get; }  
    }
}