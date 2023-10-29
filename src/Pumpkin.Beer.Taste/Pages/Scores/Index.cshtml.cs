namespace Pumpkin.Beer.Taste.Pages.ScorePages;

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.Services;
using Pumpkin.Beer.Taste.ViewModels.Scores;
using SharpRepository.Repository;

public class IndexModel : PageModel
{
    private readonly IMapper mapper;
    private readonly IClockService clockService;
    private readonly IRepository<Blind, int> blindRepository;

    public IndexModel(
        IMapper mapper,
        IClockService clockService,
        IRepository<Blind, int> blindRepository)
    {
        this.mapper = mapper;
        this.clockService = clockService;
        this.blindRepository = blindRepository;
    }

    public List<IndexViewModel> BlindItemScores { get; set; } = new();

    public IndexBlindViewModel Blind { get; set; } = null!;

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        var userId = this.User.GetUserId();
        var now = this.clockService.Now;

        var spec = Specifications
            .GetClosedBlinds(now)
            .AndAlso(Specifications.GetMemberOfBlinds(userId))
            .AndAlso(x => x.Id == id);
        spec.FetchStrategy = Strategies
            .IncludeItemsAndVotes();
        var blind = this.blindRepository.Find(spec);
        if (blind == null)
        {
            return this.NotFound();
        }

        this.Blind = this.mapper.Map<IndexBlindViewModel>(blind);

        if (!blind.BlindItems.Any(x => x.BlindVotes.Any()))
        {
            this.BlindItemScores = new();
            return this.Page();
        }

        this.BlindItemScores = this.mapper.Map<List<IndexViewModel>>(blind.BlindItems).OrderByDescending(x => x.TotalScore).ToList();

        return this.Page();
    }
}
