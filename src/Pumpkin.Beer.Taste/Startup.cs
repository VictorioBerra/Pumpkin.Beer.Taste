namespace Pumpkin.Beer.Taste;

using Ardalis.GuardClauses;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
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

        services.AddCustomKeyCloakAuthentication(configuration);

        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"), options => options.EnableRetryOnFailure()),
            ServiceLifetime.Transient);

        services.AddDbContext<MyKeysContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddDataProtection()
            .PersistKeysToDbContext<MyKeysContext>();

        services.AddSingleton(TimeProvider.System);

        services.AddSingleton<IClock>(SystemClock.Instance);

        services.AddScoped<IApplicationService, ApplicationService>();
    }

    public void ConfigureContainer(ContainerBuilder builder)
        => builder.RegisterSharpRepository(configuration.GetSection("sharpRepository"), "efCore"); // for Ef Core

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        ApplicationDbContext dbAppContext,
        MyKeysContext dbDpContext)
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

        app.UseCookiePolicy();

        app.UseRouting();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapStaticAssets();

            endpoints
                .MapRazorPages()
                .WithStaticAssets();
        });

        // Passes service provide to SharpRepository
        // https://github.com/SharpRepository/SharpRepository/blob/develop/SharpRepository.Samples.Core3Mvc/Startup.cs
        RepositoryDependencyResolver.SetDependencyResolver(app.ApplicationServices);

        dbAppContext.Database.Migrate();
        dbDpContext.Database.Migrate();
    }
}
