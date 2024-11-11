namespace Microsoft.Extensions.DependencyInjection;

using Ardalis.GuardClauses;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Pumpkin.Beer.Taste.Options;

public static class CustomServiceCollectionExtensions
{
    public static IServiceCollection AddCustomKeyCloakAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var keyCloakOptions = Guard.Against.Null(configuration.GetSection("KeyCloak").Get<KeyCloakOptions>());
        Guard.Against.NullOrEmpty(keyCloakOptions.Authority, nameof(keyCloakOptions.Authority));
        Guard.Against.NullOrEmpty(keyCloakOptions.ClientId, nameof(keyCloakOptions.ClientId));
        Guard.Against.NullOrEmpty(keyCloakOptions.ClientSecret, nameof(keyCloakOptions.ClientSecret));

        services
            .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddKeycloakWebApp(
                configureKeycloakOptions: options =>
                {
                    options.Realm = "master";
                    options.AuthServerUrl = keyCloakOptions.Authority; // "https://keycloak.obf.apps.tberra.com/";
                    options.SslRequired = "All";
                    options.Resource = keyCloakOptions.ClientId; // "pumpkinbeertaste";
                    options.VerifyTokenAudience = true;
                    options.Credentials = new KeycloakClientInstallationCredentials()
                    {
                        Secret = keyCloakOptions.ClientSecret,
                    };
                },
                configureOpenIdConnectOptions: options =>
                {
                    // we need this for front-channel sign-out
                    options.SaveTokens = true;
                    options.UsePkce = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ResponseType = "code";
                });

        return services;
    }
}
