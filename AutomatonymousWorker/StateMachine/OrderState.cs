using System;
using Automatonymous;

namespace AutomatonymousWorker.StateMachine
{
    public class OrderState :
        SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        
        public DateTime? OrderDate { get; set; }
        
        public string RejectReason { get; set; }
        
        // If using Optimistic concurrency, this property is required
        public byte[] RowVersion { get; set; }
    }
}