namespace Pumpkin.Beer.Taste.Pages.Account;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class SignoutModel : PageModel
{
    public void OnGet()
    {
    }

    public async Task OnPostAsync()
        => await this.HttpContext.SignOutAsync(new AuthenticationProperties { RedirectUri = "/" });
}
