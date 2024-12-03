namespace Microsoft.Extensions.DependencyInjection;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ardalis.GuardClauses;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Keycloak.AuthServices.Common.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.Options;
using SharpRepository.Repository;

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
                    options.Events = new OpenIdConnectEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IRepository<User, string>>();
                            var idToken = context.SecurityToken;
                            var userId = idToken.Subject;
                            if (userId != null)
                            {
                                var user = userRepository.Get(userId);
                                if (user == null)
                                {
                                    var hasEmail = idToken.Claims.TryGetClaimValue<string>("email", ClaimValueTypes.String, out var email);
                                    if (!hasEmail)
                                    {
                                        throw new InvalidOperationException("Email claim not found in token.");
                                    }

                                    var hasDisplayName = idToken.Claims.TryGetClaimValue<string>("preferred_username", ClaimValueTypes.String, out var displayName);
                                    if (!hasDisplayName)
                                    {
                                        throw new InvalidOperationException("Preferred username claim not found in token.");
                                    }

                                    user = new User
                                    {
                                        Id = userId,
                                        DisplayName = displayName!,
                                        Email = email!,
                                        WindowsTimeZoneId = "Central Standard Time",
                                    };
                                    userRepository.Add(user);

                                    context.Response.Redirect("/Account/Profile?first=1");
                                    context.HandleResponse();
                                }
                            }

                            return Task.CompletedTask;
                        },
                    };
                });

        return services;
    }
}
