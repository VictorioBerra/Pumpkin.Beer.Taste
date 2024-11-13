namespace Pumpkin.Beer.Taste.Pages.BlindPages;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.ViewModels.ManageBlind;
using SharpRepository.Repository;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class EditModel(
    ApplicationDbContext context,
    IRepository<Blind, int> blindRepository,
    IRepository<BlindItem, int> blindItemRepository) : PageModel
{
    [BindProperty]
    public EditViewModel Blind { get; set; } = null!;

    [BindProperty]
    public List<EditItemViewModel> BlindItems { get; set; } = [];

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        // Kick if it has votes
        var spec = Specifications
            .GetBlindsWithNoVotes()
            .AndAlso(x => x.Id == id);
        var blind = blindRepository.Find(spec);
        if (blind == null)
        {
            // Voting has already started, you cannot edit this tasting
            return this.RedirectToPage("/Error");
        }

        // Kick if its not theirs
        var userId = this.User.GetUserId();
        if (blind.CreatedByUserId != userId)
        {
            return this.Unauthorized();
        }

        this.Blind = new EditViewModel
        {
            Id = blind.Id,
            Name = blind.Name,
            HasVotes = blind.BlindItems.Any(y => y.BlindVotes.Count != 0),
            Started = blind.Started,
            Closed = blind.Closed,
        };

        this.BlindItems = blindItemRepository.FindAll(x => x.BlindId == id, x => new EditItemViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Ordinal = x.Ordinal,
        }).ToList();

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return this.NotFound();
        }

        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        // Kick if it has votes
        var spec = Specifications.GetBlindsWithNoVotes()
            .AndAlso(x => x.Id == id);
        var blind = blindRepository.Find(spec);
        if (blind == null)
        {
            this.ModelState.AddPageError("Voting has already started, you cannot edit this tasting.");
            return this.Page();
        }

        // Kick if it's not theirs
        var userId = this.User.GetUserId();
        if (blind.CreatedByUserId != userId)
        {
            return this.Unauthorized();
        }

        // Update the properties of the existing Blind entity
        blind.Name = this.Blind.Name;
        blind.Started = this.Blind.Started;
        blind.Closed = this.Blind.Closed;

        // Update BlindItems
        var existingBlindItems = blindItemRepository.FindAll(x => x.BlindId == id).ToList();
        var updatedBlindItems = this.BlindItems.Select((item, index) => new BlindItem
        {
            Id = item.Id,
            Name = item.Name,
            Ordinal = index,
            BlindId = blind.Id,
        }).ToList();

        // Remove old items that are not in the updated list
        foreach (var existingItem in existingBlindItems)
        {
            if (!updatedBlindItems.Any(x => x.Id == existingItem.Id))
            {
                blindItemRepository.Delete(existingItem);
            }
        }

        // Add or update items
        foreach (var updatedItem in updatedBlindItems)
        {
            var existingItem = existingBlindItems.FirstOrDefault(x => x.Id == updatedItem.Id);
            if (existingItem != null)
            {
                existingItem.Name = updatedItem.Name;
                existingItem.Ordinal = updatedItem.Ordinal;
            }
            else
            {
                blindItemRepository.Add(updatedItem);
            }
        }

        context.Attach(blind).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!this.BlindExists(this.Blind.Id))
            {
                return this.NotFound();
            }
            else
            {
                throw;
            }
        }

        return this.RedirectToPage("./Index");
    }

    private bool BlindExists(int id) => context.Blind.Any(e => e.Id == id);
}
