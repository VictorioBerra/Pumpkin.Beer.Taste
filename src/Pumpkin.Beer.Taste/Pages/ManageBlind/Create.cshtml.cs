namespace Pumpkin.Beer.Taste.Pages.BlindPages;

using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NanoidDotNet;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.ViewModels.ManageBlind;
using SharpRepository.Repository;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class CreateModel(
    IRepository<Blind, int> blindRepository) : PageModel
{
    [BindProperty]
    public CreateViewModel Blind { get; set; } = null!;

    public IActionResult OnGet() => this.Page();

    public IActionResult OnPost()
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        if (this.Blind.Started is null || this.Blind.Closed is null)
        {
            this.ModelState.AddPageError("Start and End dates are required.");
            return this.Page();
        }

        var blind = new Blind
        {
            InviteCode = Nanoid.Generate(alphabet: "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", size: 4),
            Name = this.Blind.Name,

            // Strip offset
            Started = new DateTimeOffset(this.Blind.Started.Value.DateTime, TimeSpan.Zero),
            Closed = new DateTimeOffset(this.Blind.Closed.Value.DateTime, TimeSpan.Zero),

            BlindItems = this.Blind.BlindItems.Select((x, i) => new BlindItem
            {
                Name = x.Name,
                Ordinal = i,
            }).ToList(),

            // Test this and make sure creator gets "invited"
            // An accepted invite is essentially the BlindId + CreatedByUserId (should be set automatically)
            UserInvites = [
                new UserInvite
                {
                }
            ],
        };

        blindRepository.Add(blind);

        return this.RedirectToPage("./Index");
    }
}
