namespace Pumpkin.Beer.Taste.Pages.VotePages;

using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.ViewModels.Vote;
using SharpRepository.Repository;
using SharpRepository.Repository.FetchStrategies;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class IndexModel(
    IMapper mapper,
    TimeProvider timeProvider,
    IRepository<Blind, int> blindRepository,
    IRepository<BlindVote, int> blindVoteRepository,
    IRepository<BlindItem, int> blindItemRepository) : PageModel
{
    [BindProperty]
    public IndexViewModel BlindVote { get; set; } = null!;

    public IndexBlindItemViewModel BlindItem { get; set; } = null!;

    public IndexBlindViewModel Blind { get; set; } = null!;

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        var now = timeProvider.GetLocalNow();
        var userId = this.User.GetUserId();

        // Is open and I am a member?
        var spec = Specifications.GetOpenBlinds(now)
            .AndAlso(Specifications.GetMemberOfBlinds(userId))
            .AndAlso(x => x.Id == id);

        var strat = new GenericFetchStrategy<Blind>();
        strat.Include(x => x.BlindItems.First().BlindVotes);

        spec.FetchStrategy = strat;

        var blind = blindRepository.Find(spec);
        if (blind == null)
        {
            return this.NotFound();
        }

        this.Blind = mapper.Map<IndexBlindViewModel>(blind);

        var blindItemSpec = Specifications.GetBlindsWithItemsWithNoVotesOfMine(userId)
            .AndAlso(x => x.BlindId == id);
        blindItemSpec.FetchStrategy = Strategies.IncludeBlindAndVotes();

        var nextUnvotedItem = blindItemRepository
            .FindAll(blindItemSpec)
            .OrderBy(x => x.Ordinal)
            .FirstOrDefault();

        if (nextUnvotedItem == null)
        {
            return this.Page();
        }

        this.BlindVote = new IndexViewModel()
        {
            BlindItemId = nextUnvotedItem.Id,
        };

        this.BlindItem = mapper.Map<IndexBlindItemViewModel>(nextUnvotedItem);

        return this.Page();
    }

    public IActionResult OnPost(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        var now = timeProvider.GetLocalNow();
        var userId = this.User.GetUserId();

        // Is open and not closed?
        var spec = Specifications.GetOpenBlinds(now)
            .AndAlso(x => x.Id == id);
        var strat = Strategies.IncludeItemsAndVotesAndMembers();
        spec.FetchStrategy = strat;
        var blind = blindRepository.Find(spec);
        if (blind == null)
        {
            return this.NotFound();
        }

        this.Blind = mapper.Map<IndexBlindViewModel>(blind);

        if (!this.ModelState.IsValid)
        {
            this.BlindVote = new IndexViewModel()
            {
                BlindItemId = this.BlindVote.BlindItemId,
            };

            this.BlindItem = mapper.Map<IndexBlindItemViewModel>(blindItemRepository.Get(this.BlindVote.BlindItemId));

            return this.Page();
        }

        var newVote = mapper.Map<BlindVote>(this.BlindVote);

        blindVoteRepository.Add(newVote);

        // Redirect to myself
        return this.RedirectToPage("./Index", new { Id = id });
    }
}
