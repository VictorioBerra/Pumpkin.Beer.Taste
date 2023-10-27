namespace Pumpkin.Beer.Taste.Pages.Account;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LoginModel : PageModel
{
    public async Task OnGetAsync()
        => await this.HttpContext.ChallengeAsync(new AuthenticationProperties { RedirectUri = "/" });
}
