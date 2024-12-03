namespace Pumpkin.Beer.Taste.Pages.Account;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using SharpRepository.Repository;
using TimeZoneConverter;

[Authorize]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class ProfileModel(
    IRepository<User, string> userRepository) : PageModel
{
    [Required]
    [BindProperty]
    [DisplayName("Time Zone")]
    public string IanaTimeZoneId { get; set; } = null!;

    public void OnGet()
    {
        var user = userRepository.Get(this.User.GetUserId());

        this.IanaTimeZoneId = TZConvert.WindowsToIana(user.WindowsTimeZoneId);
    }

    public void OnPost()
    {
        var user = userRepository.Get(this.User.GetUserId());

        try
        {
            var windowsTimeZoneId = TZConvert.IanaToWindows(this.IanaTimeZoneId);

            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZoneId);

            user.WindowsTimeZoneId = windowsTimeZoneId;

            userRepository.Update(user);

            this.RedirectToPage("/Index");
        }
        catch
        {
            this.ModelState.AddModelError("WindowsTimeZoneId", "Invalid time zone ID.");
            return;
        }
    }
}
