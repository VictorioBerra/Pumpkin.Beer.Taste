using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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
                        .ConfigureAppConfiguration((context, config) =>
                        {
                            var provider = new PhysicalFileProvider(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "data"));
                            Console.WriteLine("Looking for optional prod config in: " + provider.Root);
                            config.AddJsonFile(provider, "appsettings.json", optional: true, reloadOnChange: false);
                        })
                        .UseStartup<Startup>();
                });
    }
}
