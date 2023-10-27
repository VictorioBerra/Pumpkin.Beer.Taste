namespace Microsoft.Extensions.DependencyInjection;

using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using Logto.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

public static class CustomServiceCollectionExtensions
{
    public static IServiceCollection AddCustomLogToAuthentication(
        this IServiceCollection services,
        Action<LogtoOptions> configureOptions)
    {
        var logtoOptions = new LogtoOptions();
        configureOptions(logtoOptions);

        services.Configure(LogtoDefaults.AuthenticationScheme, configureOptions);
        services.AddOptions<CookieAuthenticationOptions>(LogtoDefaults.CookieScheme);

        services
          .AddAuthentication(options =>
          {
              options.DefaultScheme = LogtoDefaults.CookieScheme;
              options.DefaultChallengeScheme = LogtoDefaults.AuthenticationScheme;
              options.DefaultSignOutScheme = LogtoDefaults.AuthenticationScheme;
          })
            .AddCookie(
                LogtoDefaults.CookieScheme,
                options =>
                {
                    options.Cookie.Name = $"PumpkinTasting";
                    options.SlidingExpiration = true;
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = context => new LogtoCookieContextManager(LogtoDefaults.AuthenticationScheme, context).Handle(),
                    };
                })
            .AddOpenIdConnect(
                LogtoDefaults.AuthenticationScheme,
                options =>
                {
                    options.Authority = logtoOptions.Endpoint + "oidc";
                    options.ClientId = logtoOptions.AppId;
                    options.ClientSecret = logtoOptions.AppSecret;
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.SaveTokens = true;
                    options.UsePkce = true;
                    options.Prompt = logtoOptions.Prompt;
                    options.CallbackPath = new PathString(logtoOptions.CallbackPath);
                    options.SignedOutCallbackPath = new PathString(logtoOptions.SignedOutCallbackPath);
                    options.GetClaimsFromUserInfoEndpoint = logtoOptions.GetClaimsFromUserInfoEndpoint;
                    options.MapInboundClaims = false;
                    options.ClaimActions.MapAllExcept("nbf", "nonce", "c_hash", "at_hash");
                    options.Events = new OpenIdConnectEvents
                    {
                        OnRedirectToIdentityProviderForSignOut = async context =>
                        {
                            // Clean up the cookie when signing out.
                            await context.HttpContext.SignOutAsync(LogtoDefaults.CookieScheme);

                            // Rebuild parameters since we use <c>client_id</c> for sign-out, no need to use <c>id_token_hint</c>.
                            context.ProtocolMessage.Parameters.Remove(OpenIdConnectParameterNames.IdTokenHint);
                            context.ProtocolMessage.Parameters.Add(OpenIdConnectParameterNames.ClientId, logtoOptions.AppId);
                        },
                    };
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "username",
                        RoleClaimType = "role",
                        ValidateAudience = true,
                        ValidAudience = logtoOptions.AppId,
                        ValidateIssuer = true,
                        ValidIssuer = logtoOptions.Endpoint + "oidc",
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                    };

                    // Handle scopes
                    var scopes = new HashSet<string>(logtoOptions.Scopes)
                      {
                          "openid",
                          "offline_access",
                          "profile",
                      };

                    options.Scope.Clear();
                    foreach (var scope in scopes)
                    {
                        options.Scope.Add(scope);
                    }

                    // Handle resource
                    if (!string.IsNullOrEmpty(logtoOptions.Resource))
                    {
                        options.Resource = logtoOptions.Resource;
                    }
                });

        return services;
    }
}
