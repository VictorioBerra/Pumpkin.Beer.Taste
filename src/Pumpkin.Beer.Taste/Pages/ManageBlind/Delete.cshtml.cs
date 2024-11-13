namespace Pumpkin.Beer.Taste.Pages.BlindPages;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.ViewModels.ManageBlind;
using SharpRepository.Repository;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class DeleteModel(
    ApplicationDbContext context,
    IRepository<Blind, int> blindRepository) : PageModel
{
    [BindProperty]
    public DeleteViewModel Blind { get; set; } = null!;

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        // Kick if it has votes
        var spec = Specifications.GetBlindsWithNoVotes()
            .AndAlso(x => x.Id == id);
        var blind = blindRepository.Find(spec);
        if (blind == null)
        {
            this.ModelState.AddPageError("Voting has already started, you cannot delete this tasting.");
            return this.Page();
        }

        // Kick if its not theirs
        var userId = this.User.GetUserId();
        if (blind.CreatedByUserId != userId)
        {
            return this.Unauthorized();
        }

        this.Blind = new DeleteViewModel
        {
            Id = blind.Id,
            Name = blind.Name,
            HasVotes = blind.BlindItems.Any(y => y.BlindVotes.Count != 0),
            Started = blind.Started,
            Closed = blind.Closed,
            CreatedByUserDisplayName = blind.CreatedByUserDisplayName,
        };

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        // Kick if it has votes
        var spec = Specifications.GetBlindsWithNoVotes()
            .AndAlso(x => x.Id == id);
        var blind = blindRepository.Find(spec);
        if (blind == null)
        {
            this.ModelState.AddPageError("Voting has already started, you cannot delete this tasting.");
            return this.Page();
        }

        // Kick if its not theirs
        var userId = this.User.GetUserId();
        if (blind.CreatedByUserId != userId)
        {
            return this.Unauthorized();
        }

        context.Blind.Remove(blind);
        await context.SaveChangesAsync();

        return this.RedirectToPage("./Index");
    }
}
