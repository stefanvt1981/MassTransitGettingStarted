namespace EventHubConsumer
{
    using System;
    using System.Threading.Tasks;
    using MassTransit;
    using MassTransit.Azure.ServiceBus.Core;
    using MassTransit.EventHubIntegration;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static async Task Main()
        {
            var services = new ServiceCollection();

            services.AddMassTransit(x =>
            {
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host("Endpoint=sb://masstransitdemo1.servicebus.windows.net/;SharedAccessKeyName=MassTransitDemo;SharedAccessKey=xL7IFY1jQ557VEdyTn18CCEDouqqBeY4bSrj3DBfcJ8=");

                    cfg.ConfigureEndpoints(context);
                });
              
                x.AddRider(rider =>
                {
                    rider.AddConsumer<EventHubMessageConsumer>();

                    rider.UsingEventHub((context, k) =>
                    {
                        k.Host("Endpoint=sb://riderdemo.servicebus.windows.net/;SharedAccessKeyName=masstransitdemo;SharedAccessKey=+u6ilzDi8ys9Cn/s+VANumIhi2gJraOCoQzAFE4O80Y=");

                        k.Storage("DefaultEndpointsProtocol=https;AccountName=masstransitopslag;AccountKey=bQp/nxzcGByQ4WLwpEXqwvNHEpmPnzUHe7gNca7ARPwmS3b25+nLcZ5BQnL1o7s0XetbFxg/24hEoRZCrbQEzA==;EndpointSuffix=core.windows.net");

                        k.ReceiveEndpoint("masstransit-rider-demo", c =>
                        {
                            c.ConfigureConsumer<EventHubMessageConsumer>(context);
                        });
                    });
                });
            });
        }

        class EventHubMessageConsumer :
            IConsumer<EventHubMessage>
        {
            public Task Consume(ConsumeContext<EventHubMessage> context)
            {
                Console.Out.WriteLine($"omnomnom: {context.Message.Text}");
                return Task.CompletedTask;
            }
        }

        public interface EventHubMessage
        {
            string Text { get; }
        }
    }
}