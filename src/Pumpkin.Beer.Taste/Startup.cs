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

public class Startup
{
    public Startup(IConfiguration configuration)
        => this.Configuration = configuration;

    public IConfiguration Configuration { get; }

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

        services
            .AddRazorPages()
            .AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeFolder("/ManageBlind");
                options.Conventions.AuthorizeFolder("/Scores");
                options.Conventions.AuthorizeFolder("/Vote");
            })
            .AddRazorRuntimeCompilation();

        services.AddCustomLogToAuthentication(options =>
        {
            options.Endpoint = Guard.Against.NullOrEmpty(this.Configuration["LogTo:Endpoint"]);
            options.AppId = Guard.Against.NullOrEmpty(this.Configuration["LogTo:AppId"]);
            options.AppSecret = Guard.Against.NullOrEmpty(this.Configuration["LogTo:AppSecret"]);
            options.Scopes = Guard.Against.Null(this.Configuration.GetSection("LogTo:Scopes").Get<string[]>());
        });

        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

        services.AddAutoMapper(typeof(Startup));

        services.AddSingleton<IClockService, ClockService>();
    }

    public void ConfigureContainer(ContainerBuilder builder) => builder.RegisterSharpRepository(this.Configuration.GetSection("sharpRepository"), "efCore"); // for Ef Core

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

        dbContext.Database.EnsureCreated();
    }
}
