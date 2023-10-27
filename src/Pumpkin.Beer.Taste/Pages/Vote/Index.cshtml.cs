namespace Pumpkin.Beer.Taste.Pages.VotePages;

using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.Services;
using Pumpkin.Beer.Taste.ViewModels.Vote;
using SharpRepository.Repository;
using SharpRepository.Repository.FetchStrategies;

public class IndexModel : PageModel
{
    private readonly IMapper mapper;
    private readonly IClockService clockService;
    private readonly IRepository<Blind, int> blindRepository;
    private readonly IRepository<BlindVote, int> blindVoteRepository;
    private readonly IRepository<BlindItem, int> blindItemRepository;

    public IndexModel(
        IMapper mapper,
        IClockService clockService,
        IRepository<Blind, int> blindRepository,
        IRepository<BlindVote, int> blindVoteRepository,
        IRepository<BlindItem, int> blindItemRepository)
    {
        this.mapper = mapper;
        this.clockService = clockService;
        this.blindRepository = blindRepository;
        this.blindVoteRepository = blindVoteRepository;
        this.blindItemRepository = blindItemRepository;
    }

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

        var now = this.clockService.UtcNow;
        var userId = this.User.GetUserId();

        // Is open and not closed?
        var spec = Specifications.GetOpenBlinds(now)
            .AndAlso(x => x.Id == id);

        var strat = new GenericFetchStrategy<Blind>();
        strat.Include(x => x.BlindItems.First().BlindVotes);

        spec.FetchStrategy = strat;

        var blind = this.blindRepository.Find(spec);
        if (blind == null)
        {
            return this.NotFound();
        }

        this.Blind = this.mapper.Map<IndexBlindViewModel>(blind);

        var blindItemSpec = Specifications.GetBlindsWithItemsWithNoVotesOfMine(userId)
            .AndAlso(x => x.BlindId == id);
        blindItemSpec.FetchStrategy = Strategies.IncludeBlindAndVotes();

        var nextUnvotedItem = this.blindItemRepository
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

        this.BlindItem = this.mapper.Map<IndexBlindItemViewModel>(nextUnvotedItem);

        return this.Page();
    }

    public IActionResult OnPost(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        var now = this.clockService.UtcNow;
        var userId = this.User.GetUserId();

        // Is open and not closed?
        var spec = Specifications.GetOpenBlinds(now)
            .AndAlso(x => x.Id == id);
        var strat = Strategies.IncludeItemsAndVotes();
        spec.FetchStrategy = strat;
        var blind = this.blindRepository.Find(spec);
        if (blind == null)
        {
            return this.NotFound();
        }

        this.Blind = this.mapper.Map<IndexBlindViewModel>(blind);

        if (!this.ModelState.IsValid)
        {
            this.BlindVote = new IndexViewModel()
            {
                BlindItemId = this.BlindVote.BlindItemId,
            };

            this.BlindItem = this.mapper.Map<IndexBlindItemViewModel>(this.blindItemRepository.Get(this.BlindVote.BlindItemId));

            return this.Page();
        }

        var newVote = this.mapper.Map<BlindVote>(this.BlindVote);

        this.blindVoteRepository.Add(newVote);

        // Redirect to myself
        return this.RedirectToPage("./Index", new { Id = id });
    }
}
