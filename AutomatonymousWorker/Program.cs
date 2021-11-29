using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutomatonymousWorker
{
    public class Program
    {
        private string _connectionStringDb =
            "Server=tcp:stefan.database.windows.net,1433;Initial Catalog=MassTransit;Persist Security Info=False;User ID=stefan;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => { services.AddHostedService<Worker>(); });
    }
}