namespace Pumpkin.Beer.Taste.Pages.Tastings;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Eblindtensions;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.ViewModels.Tastings;
using SharpRepository.Repository;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class ClosedModel(
    TimeProvider timeProvider,
    IRepository<User, string> userRepository,
    IRepository<Blind, int> blindRepository) : PageModel
{
    public List<IndexViewModel> ClosedBlinds { get; set; } = [];

    public void OnGet()
    {
        var userId = this.User.GetUserId();
        var user = userRepository.Get(userId);
        var now = timeProvider.GetUtcNow();

        this.SetClosedBlinds(user, now);
    }

    private void SetClosedBlinds(User user, DateTimeOffset now)
    {
        var spec = Specifications.GetMemberOfBlinds(user.Id);
        spec.FetchStrategy = Strategies.IncludeItemsAndVotesAndMembers();

        var closedBlinds = blindRepository.FindAll(spec).ToList();

        this.ClosedBlinds = closedBlinds
            .Where(x => x.HasEventStarted(now, user.WindowsTimeZoneId))
            .Select(x => new IndexViewModel
            {
                Id = x.Id,
                Name = x.Name,
                HasVotes = x.BlindItems.Any(x => x.BlindVotes.Count != 0),
                Started = x.StartedUtc,
                Closed = x.ClosedUtc,
                CreatedByUserDisplayName = x.CreatedByUserDisplayName,
                NumMembers = x.UserInvites.Count,
            }).ToList();
    }
}
