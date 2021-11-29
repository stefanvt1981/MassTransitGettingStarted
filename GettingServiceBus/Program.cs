using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GettingServiceBus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<MessageConsumer>();

                        x.UsingAzureServiceBus((context,cfg) =>
                        {
                            var connectionString = "Endpoint=sb://masstransitdemo1.servicebus.windows.net/;SharedAccessKeyName=MassTransitDemo;SharedAccessKey=xL7IFY1jQ557VEdyTn18CCEDouqqBeY4bSrj3DBfcJ8=";
                            cfg.Host(connectionString);

                            cfg.ConfigureEndpoints(context);
                        });
                    });
                    
                    services.AddMassTransitHostedService(true);
                    services.AddHostedService<Worker>();
                });
    }
}
