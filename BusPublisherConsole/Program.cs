using MassTransit;
using System;
using System.Threading.Tasks;
using AutomatonymousWorker.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BusPublisherConsole
{
    class Program
    {
        private static Microsoft.Extensions.Hosting.IHost _host;
        
        static void Main(string[] args)
        {
            _host = CreateHostBuilder(args).Build();
            Task.Run(() => _host.Run());

            StartCommandLoop(_host.Services.GetService<IBus>());
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.UsingRabbitMq((context,cfg) =>
                        {
                            cfg.ConfigureEndpoints(context);
                        });
                    });
                    
                    services.AddMassTransitHostedService(true);
                });
        
        private static void StartCommandLoop(IBus bus)
        {
            var orderId = Guid.NewGuid();
            var lastCommand = "";
            
            PrintCommands(orderId.ToString(), lastCommand);
            
            var command = Console.ReadLine();

            while (command != null && command.ToLower() != "quit")
            {
                
                switch (command)
                {
                    case "0":
                        orderId = Guid.NewGuid();
                        lastCommand = $"New OrderId: {orderId}";
                        break;
                    case "1":
                        lastCommand = $"Send OrderSubmitted OrderId: {orderId}";
                        bus.Publish<OrderSubmitted>(new { OrderId = orderId, OrderDate = DateTime.Now });
                        break;
                    case "2":
                        lastCommand = $"Send OrderAccepted OrderId: {orderId}";
                        bus.Publish<OrderAccepted>(new { OrderId = orderId });
                        break;
                    case "3":
                        lastCommand = $"Send OrderRejected OrderId: {orderId}";
                        bus.Publish<OrderRejected>(new { OrderId = orderId, RejectReason = "Stupid order..." });
                        break;
                    case "4":
                        lastCommand = $"Send OrderCancelled OrderId: {orderId}";
                        bus.Publish<OrderCancelled>(new { OrderId = orderId });
                        break;
                    case "5":
                        lastCommand = $"Send OrderCompleted OrderId: {orderId}";
                        bus.Publish<OrderCompleted>(new { OrderId = orderId });
                        break;
                    default:
                        Console.WriteLine($"Commando '{command}' niet herkent.");
                        break;
                }
                
                PrintCommands(orderId.ToString(), lastCommand);
                
                command = Console.ReadLine();
            }

            Console.WriteLine("Bye..");
        }

        private static void PrintCommands(string orderId, string lastCommand)
        {
            Console.Clear();
            Console.WriteLine($"Enter 0 to generate new OrderId. Current: '{orderId}'");
            Console.WriteLine("Enter 1 to send OrderSubmitted message.");
            Console.WriteLine("Enter 2 to send OrderAccepted message.");
            Console.WriteLine("Enter 3 to send OrderRejected message.");
            Console.WriteLine("Enter 4 to send OrderCancelled message.");
            Console.WriteLine("Enter 5 to send Order message.");
            Console.WriteLine("");
            Console.WriteLine("Enter 'Quit' to exit.");
            Console.WriteLine("");
            Console.WriteLine(lastCommand);
        }
    }
}