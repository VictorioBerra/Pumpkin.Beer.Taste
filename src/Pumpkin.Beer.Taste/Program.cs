using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pumpkin.Beer.Taste.Data;

namespace Pumpkin.Beer.Taste
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost webHost = CreateHostBuilder(args).Build();

            using (var scope = webHost.Services.CreateScope())
            {
                var identitySeed = scope.ServiceProvider.GetRequiredService<IdentitySeed>();
                var dataSeed = scope.ServiceProvider.GetRequiredService<DataSeed>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                context.Database.Migrate();

                await identitySeed.CreateRoles();

                var seed = args.Contains("/seed");
                if (seed)
                {
                    await dataSeed.Seed();
                }
            }

            await webHost.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .ConfigureAppConfiguration(config => 
                        {
                            config.AddJsonFile("../data/appsettings.json", optional: true, reloadOnChange: true);
                        });
                });
    }
}
