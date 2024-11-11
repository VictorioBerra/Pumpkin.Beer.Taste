namespace Pumpkin.Beer.Taste.Pages.Account;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class LogoutModel(
    ILogger<LogoutModel> logger) : PageModel
{
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.User.Identity!.IsAuthenticated)
        {
            return this.Challenge(OpenIdConnectDefaults.AuthenticationScheme);
        }

        var idToken = await this.HttpContext.GetTokenAsync("id_token");

        var authResult = this
            .HttpContext.Features.Get<IAuthenticateResultFeature>()
            ?.AuthenticateResult;

        var tokens = authResult!.Properties!.GetTokens();

        var tokenNames = tokens.Select(token => token.Name).ToArray();

        logger.LogInformation("Token Names: {TokenNames}", string.Join(", ", tokenNames));

        return this.SignOut(
            new AuthenticationProperties
            {
                RedirectUri = "/",
                Items =
                {
                    { "id_token_hint", idToken },
                },
            },
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}
