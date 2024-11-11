namespace Pumpkin.Beer.Taste.Pages.BlindPages;

using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.Services;
using Pumpkin.Beer.Taste.ViewModels.ManageBlind;
using SharpRepository.Repository;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class CloseModel(
    ApplicationDbContext context,
    IMapper mapper,
    IClockService clockService,
    IRepository<Blind, int> blindRepository) : PageModel
{
    [BindProperty]
    public CloseViewModel Blind { get; set; } = null!;

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        var blind = blindRepository.Get((int)id);
        if (blind == null)
        {
            return this.NotFound();
        }

        // Kick if its not theirs
        var userId = this.User.GetUserId();
        if (blind.CreatedByUserId != userId)
        {
            return this.Unauthorized();
        }

        this.Blind = mapper.Map<CloseViewModel>(blind);

        if (this.Blind == null)
        {
            return this.NotFound();
        }

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        var now = clockService.Now;

        var blind = blindRepository.Get((int)id);
        if (blind == null)
        {
            return this.NotFound();
        }

        // Kick if its not theirs
        var userId = this.User.GetUserId();
        if (blind.CreatedByUserId != userId)
        {
            return this.Unauthorized();
        }

        this.Blind = mapper.Map<CloseViewModel>(blind);

        if (this.Blind != null)
        {
            blind.Closed = now.UtcDateTime;
            context.Attach(blind).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        return this.RedirectToPage("./Index");
    }
}
