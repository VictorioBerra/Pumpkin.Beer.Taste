namespace Microsoft.Extensions.DependencyInjection;

using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

public static class CustomServiceCollectionExtensions
{
    public static IServiceCollection AddCustomKeyCloakAuthentication(
        this IServiceCollection services)
    {
        services
            .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddKeycloakWebApp(
                configureKeycloakOptions: options =>
                {
                    options.Realm = "master";
                    options.AuthServerUrl = "https://keycloak.obf.apps.tberra.com/";
                    options.SslRequired = "All";
                    options.Resource = "pumpkinbeertaste";
                    options.VerifyTokenAudience = true;
                    options.Credentials = new KeycloakClientInstallationCredentials()
                    {
                        Secret = Environment.GetEnvironmentVariable("KeyCloakSecret")!,
                    };
                },
                configureOpenIdConnectOptions: options =>
                {
                    options.UsePkce = true;
                    options.ResponseType = "code";
                });

        return services;
    }
}
