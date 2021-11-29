namespace EventHubProducer
{
    using System;
    using System.Threading;
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
                    rider.UsingEventHub((context, k) =>
                    {
                        k.Host("Endpoint=sb://riderdemo.servicebus.windows.net/;SharedAccessKeyName=masstransitdemo;SharedAccessKey=+u6ilzDi8ys9Cn/s+VANumIhi2gJraOCoQzAFE4O80Y=");

                        k.Storage("DefaultEndpointsProtocol=https;AccountName=masstransitopslag;AccountKey=bQp/nxzcGByQ4WLwpEXqwvNHEpmPnzUHe7gNca7ARPwmS3b25+nLcZ5BQnL1o7s0XetbFxg/24hEoRZCrbQEzA==;EndpointSuffix=core.windows.net");
                    });
                });
            });

            var provider = services.BuildServiceProvider(true);

            var busControl = provider.GetRequiredService<IBusControl>();

            await busControl.StartAsync(new CancellationTokenSource(10000).Token);

            var serviceScope = provider.CreateScope();

            var producerProvider = serviceScope.ServiceProvider.GetRequiredService<IEventHubProducerProvider>();
            var producer = await producerProvider.GetProducer("masstransit-rider-demo");

            await producer.Produce<EventHubMessage>(new { Text = "Hello, Computer." });
        }

        public interface EventHubMessage
        {
            string Text { get; }
        }
    }
}