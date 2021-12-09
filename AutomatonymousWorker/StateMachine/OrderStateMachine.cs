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
            InstanceState(x => x.CurrentState, Submitted, Accepted, Rejected, Cancelled);
            
            Event(() => OrderSubmitted, x => x.CorrelateById(c => c.Message.OrderId));
            Event(() => OrderAccepted, x => x.CorrelateById(c => c.Message.OrderId));
            Event(() => OrderRejected, x => x.CorrelateById(c => c.Message.OrderId));
            Event(() => OrderCancelled, x => x.CorrelateById(c => c.Message.OrderId));
            Event(() => OrderCompleted, x => x.CorrelateById(c => c.Message.OrderId));

            Initially(
                When(OrderSubmitted)
                    .Then(x =>
                    {
                        x.Instance.OrderDate = x.Data.OrderDate;
                        Console.WriteLine("Order submitted");
                    })
                    .TransitionTo(Submitted),
                When(OrderAccepted)
                    .Then(context => Console.WriteLine("Accept order"))
                    .TransitionTo(Accepted),
                When(OrderCancelled)
                    .Then(context => Console.WriteLine("Cancel order"))
                    .TransitionTo(Cancelled));

            During(Submitted,
                When(OrderAccepted)
                    .Then(context => Console.WriteLine("Accept order"))
                    .TransitionTo(Accepted),
                When(OrderRejected)
                    .Then(context =>
                    {
                        context.Instance.RejectReason = context.Data.RejectReason;
                        Console.WriteLine("Reject order");
                    })
                    .TransitionTo(Rejected)
                );

            During(Accepted,
                When(OrderSubmitted)
                    .Then(x =>
                    {
                        x.Instance.OrderDate = x.Data.OrderDate;
                        Console.WriteLine("Order date updated (SubmitOrder)");
                    }),
                When(OrderCancelled)
                    .Then(context => Console.WriteLine("Cancel order"))
                    .TransitionTo(Cancelled));
            
            During(Rejected, 
                Ignore(OrderSubmitted), Ignore(OrderCancelled));
            
            DuringAny(
                When(OrderCompleted)
                    .Then(context => Console.WriteLine("Order completed"))
                    .Finalize());
        }
        
        public Event<OrderSubmitted> OrderSubmitted { get; private set; }
        public Event<OrderAccepted> OrderAccepted { get; private set; }
        public Event<OrderRejected> OrderRejected { get; private set; }
        public Event<OrderCancelled> OrderCancelled { get; private set; }
        public Event<OrderCompleted> OrderCompleted { get; private set; }

    }
}