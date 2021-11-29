using System;
using Automatonymous;

namespace AutomatonymousWorker.StateMachine
{
    public class OrderState :
        SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public State CurrentState { get; set; }
        
        public DateTime? OrderDate { get; set; }
        
        public string RejectReason { get; set; }
    }
}