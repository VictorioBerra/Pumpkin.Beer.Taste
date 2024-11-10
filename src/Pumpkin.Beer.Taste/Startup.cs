namespace Pumpkin.Beer.Taste;

using Ardalis.GuardClauses;
using Autofac;
using Logto.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Services;
using SharpRepository.Ioc.Autofac;
using SharpRepository.Repository.Ioc;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.Configure<ForwardedHeadersOptions>(
            options =>
            {
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
                options.ForwardedHeaders = ForwardedHeaders.All;
            });

        services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential
            // cookies is needed for a given request.
            // Setting to false as right now we do not track anything https://law.stackexchange.com/a/85353
            options.CheckConsentNeeded = context => false;

            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services
            .AddRazorPages()
            .AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeFolder("/ManageBlind");
                options.Conventions.AuthorizeFolder("/Scores");
                options.Conventions.AuthorizeFolder("/Vote");
            })
            .AddRazorRuntimeCompilation();

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        services.AddCustomKeyCloakAuthentication();

        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"), options => options.EnableRetryOnFailure()),
            ServiceLifetime.Transient);

        services.AddAutoMapper(typeof(Startup));

        services.AddSingleton<IClockService, ClockService>();
    }

    public void ConfigureContainer(ContainerBuilder builder)
        => builder.RegisterSharpRepository(configuration.GetSection("sharpRepository"), "efCore"); // for Ef Core

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseForwardedHeaders();

        app.UseStaticFiles();
        app.UseCookiePolicy();

        app.UseRouting();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapRazorPages());

        // Passes service provide to SharpRepository
        // https://github.com/SharpRepository/SharpRepository/blob/develop/SharpRepository.Samples.Core3Mvc/Startup.cs
        RepositoryDependencyResolver.SetDependencyResolver(app.ApplicationServices);

        dbContext.Database.Migrate();
    }
}
