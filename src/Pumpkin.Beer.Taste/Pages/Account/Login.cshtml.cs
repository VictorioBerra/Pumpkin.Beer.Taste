namespace Pumpkin.Beer.Taste.Pages.Account;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class LoginModel : PageModel
{
    public async Task OnGetAsync()
        => await this.HttpContext.ChallengeAsync(new AuthenticationProperties { RedirectUri = "/" });
}
