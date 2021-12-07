using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutomatonymousWorker.Persistance;
using AutomatonymousWorker.StateMachine;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.Saga;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace AutomatonymousWorker
{
    public class Program
    {
        private static string your_password = "SophieHelena2008!";
        private static string _connectionStringDb = $"Server=tcp:stefan.database.windows.net,1433;Initial Catalog=MassTransit;Persist Security Info=False;User ID=stefan;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
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
                        x.AddSagaStateMachine<OrderStateMachine, OrderState>()
                            .EntityFrameworkRepository(r =>
                            {
                                r.ConcurrencyMode = ConcurrencyMode.Pessimistic; // or use Optimistic, which requires RowVersion

                                r.AddDbContext<DbContext, OrderStateDbContext>((provider,builder) =>
                                {
                                    builder.UseSqlServer(_connectionStringDb, m =>
                                    {
                                        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                                        m.MigrationsHistoryTable($"__{nameof(OrderStateDbContext)}");
                                    });
                                });
                            });
                        x.UsingRabbitMq((context,cfg) =>
                        {
                            cfg.ConfigureEndpoints(context);
                        });
                    });
                    
                    services.AddMassTransitHostedService(true);
                });
    }
}