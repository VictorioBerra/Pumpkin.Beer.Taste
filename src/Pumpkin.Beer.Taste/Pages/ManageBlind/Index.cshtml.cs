namespace Pumpkin.Beer.Taste.Pages.BlindPages;

using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Eblindtensions;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.ViewModels.ManageBlind;
using SharpRepository.Repository;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class IndexModel(
    IMapper mapper,
    TimeProvider timeProvider,
    IRepository<Blind, int> blindRepository) : PageModel
{
    public List<IndexViewModel> Blinds { get; set; } = [];

    public void OnGet()
    {
        var userId = this.User.GetUserId();

        var strat = Specifications.GetOwnedBlinds(userId);
        strat.FetchStrategy = Strategies.IncludeItemsAndVotesAndMembers();

        var blinds = blindRepository.FindAll(strat);

        this.Blinds = [.. mapper.Map<List<IndexViewModel>>(blinds).OrderByDescending(x => x.Closed is not null)];

        var now = timeProvider.GetLocalNow();

        foreach (var blind in this.Blinds)
        {
            var origBlind = blinds.Single(x => x.Id == blind.Id);
            blind.IsOpen = origBlind.IsOpen(now);
        }
    }
}
