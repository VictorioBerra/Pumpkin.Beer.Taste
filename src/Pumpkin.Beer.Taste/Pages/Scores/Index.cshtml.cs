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
using Pumpkin.Beer.Taste.ViewModels.Scores;
using SharpRepository.Repository;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class IndexModel(
    IMapper mapper,
    TimeProvider timeProvider,
    IRepository<Blind, int> blindRepository) : PageModel
{
    public List<IndexViewModel> BlindItemScores { get; set; } = [];

    public IndexBlindViewModel Blind { get; set; } = null!;

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        var userId = this.User.GetUserId();
        var now = timeProvider.GetLocalNow();

        var spec = Specifications
            .GetClosedBlinds(now)
            .AndAlso(Specifications.GetMemberOfBlinds(userId))
            .AndAlso(x => x.Id == id);
        spec.FetchStrategy = Strategies
            .IncludeItemsAndVotes();
        var blind = blindRepository.Find(spec);
        if (blind == null)
        {
            return this.NotFound();
        }

        this.Blind = mapper.Map<IndexBlindViewModel>(blind);

        if (!blind.BlindItems.Any(x => x.BlindVotes.Count != 0))
        {
            this.BlindItemScores = [];
            return this.Page();
        }

        this.BlindItemScores = [.. mapper.Map<List<IndexViewModel>>(blind.BlindItems).OrderByDescending(x => x.TotalScore)];

        return this.Page();
    }
}
