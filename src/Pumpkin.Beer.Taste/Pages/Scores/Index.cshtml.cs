namespace Pumpkin.Beer.Taste.Pages.ScorePages;

using System.Collections.Generic;
using System.Linq;
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
    TimeProvider timeProvider,
    IRepository<User, string> userRepository,
    IRepository<Blind, int> blindRepository) : PageModel
{
    public ScoreBlindViewModel Blind { get; set; } = null!;

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        var userId = this.User.GetUserId();
        var user = userRepository.Get(userId);
        var now = timeProvider.GetUtcNow();

        var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.WindowsTimeZoneId);
        var userCurrentTime = TimeZoneInfo.ConvertTimeFromUtc(now.DateTime, userTimeZone);
        var spec = Specifications.GetClosedBlinds(userCurrentTime, user.WindowsTimeZoneId)
            .AndAlso(Specifications.GetMemberOfBlinds(userId))
            .AndAlso(x => x.Id == id);
        spec.FetchStrategy = Strategies
            .IncludeItemsAndVotes();

        this.Blind = blindRepository.Find(spec, x => new ScoreBlindViewModel
        {
            Id = x.Id,
            Name = x.Name,
            HasVotes = x.BlindItems.Any(x => x.BlindVotes.Count != 0),
            Started = TimeZoneInfo.ConvertTimeFromUtc(x.StartedUtc, userTimeZone),
            Closed = TimeZoneInfo.ConvertTimeFromUtc(x.ClosedUtc, userTimeZone),
            BlindItemScores = x.BlindItems.Select(y => new ScoreViewModel
            {
                TotalScore = y.BlindVotes.Count,
                AverageScore = y.BlindVotes.Sum(x => x.Score),
                AmountOfVotes = y.BlindVotes.Average(x => x.Score),
                BlindItem = new ScoreBlindItemViewModel
                {
                    Id = x.Id,
                    Name = y.Name,
                    Ordinal = y.Ordinal,
                    Votes = y.BlindVotes
                        .Select(z => new ScoreBlindItemVoteViewModel
                        {
                            Score = z.Score,
                            Note = z.Note,
                            Public = z.Public,
                            CreatedByUserDisplayName = x.CreatedByUserDisplayName,
                        }).ToList(),
                },
            }).ToList(),
        });

        if (this.Blind == null)
        {
            return this.NotFound();
        }

        return this.Page();
    }
}
