using AutomatonymousWorker.StateMachine;
using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutomatonymousWorker.Persistance
{
    public class OrderStateMap : SagaClassMap<OrderState>
    {
        protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState).HasMaxLength(64);
            entity.Property(x => x.OrderDate);
            entity.Property(x => x.RejectReason);

            // If using Optimistic concurrency, otherwise remove this property
            entity.Property(x => x.RowVersion).IsRowVersion();
        }
    }
}