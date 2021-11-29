using System;
using Automatonymous;
using AutomatonymousWorker.Events;

namespace AutomatonymousWorker.StateMachine
{
    public class OrderStateMachine :
        MassTransitStateMachine<OrderState>
    {
        public State Submitted { get; private set; }
        public State Accepted { get; private set; }
        public State Rejected { get; private set; }
        
        public State Cancelled { get; private set; }
        
        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);
            
            Initially(
                When(SubmitOrder)
                    .Then(x =>
                    {
                        x.Instance.OrderDate = x.Data.OrderDate;
                        Console.WriteLine("Order submitted");
                    })
                    .TransitionTo(Submitted),
                When(OrderAccepted)
                    .Then(context => Console.WriteLine("Accept order"))
                    .TransitionTo(Accepted),
                When(CancelOrder)
                    .Then(context => Console.WriteLine("Cancel order"))
                    .TransitionTo(Cancelled));

            During(Submitted,
                When(OrderAccepted)
                    .Then(context => Console.WriteLine("Accept order"))
                    .TransitionTo(Accepted),
                When(OrderRejected)
                    .Then(context => Console.WriteLine("Reject order"))
                    .TransitionTo(Rejected)
                );

            During(Accepted,
                When(SubmitOrder)
                    .Then(x =>
                    {
                        x.Instance.OrderDate = x.Data.OrderDate;
                        Console.WriteLine("Order date updated (SubmitOrder)");
                    }),
                When(CancelOrder)
                    .Then(context => Console.WriteLine("Cancel order"))
                    .TransitionTo(Cancelled));
            
            During(Rejected, 
                Ignore(SubmitOrder), Ignore(CancelOrder));
            
            DuringAny(
                When(OrderCompleted)
                    .Then(context => Console.WriteLine("Order completed"))
                    .Finalize());
        }
        
        public Event<SubmitOrder> SubmitOrder { get; private set; }
        public Event<OrderAccepted> OrderAccepted { get; private set; }
        public Event<OrderRejected> OrderRejected { get; private set; }
        public Event<CancelOrder> CancelOrder { get; private set; }
        public Event<OrderCompleted> OrderCompleted { get; private set; }

    }
}