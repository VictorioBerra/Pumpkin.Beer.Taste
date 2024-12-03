namespace Pumpkin.Beer.Taste.Pages.VotePages;

using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.Services;
using Pumpkin.Beer.Taste.ViewModels.Vote;
using SharpRepository.Repository;
using SharpRepository.Repository.FetchStrategies;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class IndexModel(
    TimeProvider timeProvider,
    IApplicationService applicationService,
    IRepository<Blind, int> blindRepository,
    IRepository<User, string> userRepository,
    IRepository<BlindVote, int> blindVoteRepository,
    IRepository<BlindItem, int> blindItemRepository) : PageModel
{
    [BindProperty]
    public IndexViewModel BlindVote { get; set; } = null!;

    public IndexBlindItemViewModel BlindItem { get; set; } = null!;

    public IndexBlindViewModel Blind { get; set; } = null!;

    public IActionResult OnGet(int? id, string inviteCode)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        if (!string.IsNullOrWhiteSpace(inviteCode))
        {
            var inviteAcceptResult = applicationService.AcceptInvite(this.User, inviteCode);

            if (inviteAcceptResult.Status is Ardalis.Result.ResultStatus.Error)
            {
                // TODO: Maybe show them the error via this.ModelState.AddPageError(inviteAcceptResult);?
                return this.RedirectToPage("/Index");
            }
        }

        var userId = this.User.GetUserId();
        var user = userRepository.Get(userId);
        var now = timeProvider.GetUtcNow();

        var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.WindowsTimeZoneId);
        var userCurrentTime = TimeZoneInfo.ConvertTimeFromUtc(now.DateTime, userTimeZone);
        var spec = Specifications.GetOpenBlinds(userCurrentTime, user.WindowsTimeZoneId)
            .AndAlso(Specifications.GetMemberOfBlinds(userId))
            .AndAlso(x => x.Id == id);

        var strat = new GenericFetchStrategy<Blind>();
        strat.Include(x => x.BlindItems.First().BlindVotes);

        spec.FetchStrategy = strat;

        this.Blind = blindRepository.Find(spec, x => new IndexBlindViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Started = TimeZoneInfo.ConvertTimeFromUtc(x.StartedUtc, userTimeZone),
            Closed = TimeZoneInfo.ConvertTimeFromUtc(x.ClosedUtc, userTimeZone),
        });
        if (this.Blind == null)
        {
            return this.NotFound();
        }

        var blindItemSpec = Specifications.GetBlindsWithItemsWithNoVotesOfMine(userId)
            .AndAlso(x => x.BlindId == id);
        blindItemSpec.FetchStrategy = Strategies.IncludeBlindAndVotes();

        var blindItem = blindItemRepository
            .FindAll(blindItemSpec, x => new IndexBlindItemViewModel
            {
                Id = x.Id,
                Ordinal = x.Ordinal,
            });

        if (blindItem == null || !blindItem.Any())
        {
            return this.Page();
        }

        this.BlindItem =
            blindItem
            .OrderBy(x => x.Ordinal)
            .FirstOrDefault()!;

        this.BlindVote = new IndexViewModel()
        {
            BlindItemId = this.BlindItem.Id,
        };

        return this.Page();
    }

    public IActionResult OnPost(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        var userId = this.User.GetUserId();
        var user = userRepository.Get(userId);
        var now = timeProvider.GetUtcNow();

        // Is open and not closed?
        var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.WindowsTimeZoneId);
        var userCurrentTime = TimeZoneInfo.ConvertTimeFromUtc(now.DateTime, userTimeZone);
        var spec = Specifications.GetOpenBlinds(userCurrentTime, user.WindowsTimeZoneId)
            .AndAlso(Specifications.GetMemberOfBlinds(userId))
            .AndAlso(x => x.Id == id);
        var strat = Strategies.IncludeItemsAndVotesAndMembers();
        spec.FetchStrategy = strat;
        var blind = blindRepository.Find(spec, x => new IndexBlindViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Started = TimeZoneInfo.ConvertTimeFromUtc(x.StartedUtc, userTimeZone),
            Closed = TimeZoneInfo.ConvertTimeFromUtc(x.ClosedUtc, userTimeZone),
        });

        if (blind == null)
        {
            return this.NotFound();
        }

        this.Blind = blind;

        if (!this.ModelState.IsValid)
        {
            this.BlindVote = new IndexViewModel()
            {
                BlindItemId = this.BlindVote.BlindItemId,
            };

            this.BlindItem = new IndexBlindItemViewModel()
            {
                Id = this.BlindVote.BlindItemId,
            };

            return this.Page();
        }

        var newVote = new BlindVote
        {
            Score = this.BlindVote.Score,
            Public = this.BlindVote.Public,
            Note = this.BlindVote.Note,
            BlindItemId = this.BlindVote.BlindItemId,
        };

        blindVoteRepository.Add(newVote);

        // Redirect to myself
        return this.RedirectToPage("./Index", new { Id = id });
    }
}
