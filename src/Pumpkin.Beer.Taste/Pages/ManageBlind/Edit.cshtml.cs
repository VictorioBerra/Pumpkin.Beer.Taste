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
using TimeZoneConverter;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class EditModel(
    ApplicationDbContext context,
    TimeProvider timeProvider,
    IRepository<User, string> userRepository,
    IRepository<Blind, int> blindRepository) : PageModel
{
    [BindProperty]
    public EditViewModel Blind { get; set; } = null!;

    [BindProperty]
    public List<EditItemViewModel> BlindItems { get; set; } = [];

    public string CurrentUserProfileIANATimeZoneId { get; set; } = null!;

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
        spec.FetchStrategy.Include(x => x.BlindItems);
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

        var now = timeProvider.GetUtcNow();
        var user = userRepository.Get(userId);

        var startedWindowsTimeZone = TimeZoneInfo.FindSystemTimeZoneById(blind.StartedWindowsTimeZoneId);
        var startedUtc = TimeZoneInfo.ConvertTimeToUtc(blind.StartedUtc, startedWindowsTimeZone);

        var closedWindowsTimeZone = TimeZoneInfo.FindSystemTimeZoneById(blind.ClosedWindowsTimeZoneId);
        var closedUtc = TimeZoneInfo.ConvertTimeToUtc(blind.ClosedUtc, closedWindowsTimeZone);

        var startedAndClosedIANATimeZoneId = TZConvert.WindowsToIana(startedWindowsTimeZone.Id);

        this.Blind = new EditViewModel
        {
            Id = blind.Id,
            Name = blind.Name,
            HasVotes = blind.BlindItems.Any(y => y.BlindVotes.Count != 0),
            Started = TimeZoneInfo.ConvertTimeFromUtc(blind.StartedUtc, startedWindowsTimeZone),
            Closed = TimeZoneInfo.ConvertTimeFromUtc(blind.ClosedUtc, closedWindowsTimeZone),

            // Just using startedWindowsTimeZone as both for now, maybe one day we'll allow setting different time zones for start and close
            // Should work right now though if we did, just wanted to simplify UI.
            StartedAndClosedIANATimeZoneId = startedAndClosedIANATimeZoneId,
        };

        this.BlindItems = blind.BlindItems.Select(x => new EditItemViewModel
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
        spec.FetchStrategy.Include(x => x.BlindItems);
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

        var now = timeProvider.GetUtcNow();
        var user = userRepository.Get(userId);

        var windowsTimeZoneId = TZConvert.IanaToWindows(this.Blind.StartedAndClosedIANATimeZoneId);
        var windowsTimeZone = TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZoneId);
        var startedUtc = TimeZoneInfo.ConvertTimeToUtc(this.Blind.Started, windowsTimeZone);
        var endedUtc = TimeZoneInfo.ConvertTimeToUtc(this.Blind.Closed, windowsTimeZone);

        // Update the properties of the existing Blind entity
        blind.Name = this.Blind.Name;
        blind.StartedUtc = startedUtc;
        blind.ClosedUtc = endedUtc;
        blind.StartedWindowsTimeZoneId = windowsTimeZoneId;
        blind.ClosedWindowsTimeZoneId = windowsTimeZoneId;

        // Update BlindItems
        var updatedBlindItems = this.BlindItems.Select((item, index) => new BlindItem
        {
            Id = item.Id,
            Name = item.Name,
            Ordinal = index,
            BlindId = blind.Id,
        }).ToList();

        // Remove old items that are not in the updated list
        var itemsToRemove = blind.BlindItems.Where(existingItem => !updatedBlindItems.Any(x => x.Id == existingItem.Id)).ToList();
        foreach (var item in itemsToRemove)
        {
            blind.BlindItems.Remove(item);
            context.Entry(item).State = EntityState.Deleted;
        }

        // Add or update items
        foreach (var updatedItem in updatedBlindItems)
        {
            var existingItem = blind.BlindItems.FirstOrDefault(x => x.Id == updatedItem.Id);
            if (existingItem != null)
            {
                existingItem.Name = updatedItem.Name;
                existingItem.Ordinal = updatedItem.Ordinal;
                context.Entry(existingItem).State = EntityState.Modified;
            }
            else
            {
                blind.BlindItems.Add(updatedItem);
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
