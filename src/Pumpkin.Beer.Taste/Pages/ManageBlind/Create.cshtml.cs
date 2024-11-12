namespace Pumpkin.Beer.Taste.Pages.BlindPages;

using System.Linq;
using AutoMapper;
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
    IMapper mapper,
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

        var blind = mapper.Map<Blind>(this.Blind);
        blind.InviteCode = Nanoid.Generate(alphabet: "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", size: 4);

        if (this.Blind.Started is null || this.Blind.Closed is null)
        {
            this.ModelState.AddPageError("Start and End dates are required.");
            return this.Page();
        }

        // Strip offset
        blind.Started = new DateTimeOffset(this.Blind.Started.Value.DateTime, TimeSpan.Zero);
        blind.Closed = new DateTimeOffset(this.Blind.Closed.Value.DateTime, TimeSpan.Zero);

        var blindItems = blind.BlindItems.ToList();
        for (var i = 0; i < blindItems.Count; i++)
        {
            blindItems[i].Ordinal = i;
        }

        blind.BlindItems = blindItems;
        blind.UserInvites.Add(new UserInvite
        {
            Blind = blind,
        });

        blindRepository.Add(blind);

        return this.RedirectToPage("./Index");
    }
}
