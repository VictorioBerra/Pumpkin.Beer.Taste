using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Pumpkin.Beer.Taste.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Microsoft.AspNetCore.HttpOverrides;
using Pumpkin.Beer.Taste.Services;
using Autofac;
using SharpRepository.Ioc.Autofac;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SharpRepository.Repository.Ioc;

namespace Pumpkin.Beer.Taste
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services
                .AddDefaultIdentity<IdentityUser>(options => {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services
                .AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/ManageBlind");
                })
                .AddRazorRuntimeCompilation();

            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseMySql(Configuration.GetConnectionString("MySQLConnection"), mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new Version(5, 7), ServerType.MySql); // replace with your Server Version and Type
                });
            }, ServiceLifetime.Transient);

            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<IdentitySeed>();
            services.AddTransient<DataSeed>();
            services.AddSingleton<IClockService, ClockService>();

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterSharpRepository(Configuration.GetSection("sharpRepository"), "efCore"); // for Ef Core
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            // Passes service provide to SharpRepository
            // https://github.com/SharpRepository/SharpRepository/blob/develop/SharpRepository.Samples.Core3Mvc/Startup.cs
            RepositoryDependencyResolver.SetDependencyResolver(app.ApplicationServices);
        }
    }
}
