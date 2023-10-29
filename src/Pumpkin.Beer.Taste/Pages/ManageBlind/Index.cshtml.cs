namespace Pumpkin.Beer.Taste.Pages.BlindPages;

using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.ViewModels.ManageBlind;
using SharpRepository.Repository;

public class IndexModel : PageModel
{
    private readonly IMapper mapper;
    private readonly IRepository<Blind, int> blindRepository;

    public IndexModel(
        IMapper mapper,
        IRepository<Blind, int> blindRepository)
    {
        this.mapper = mapper;
        this.blindRepository = blindRepository;
    }

    public List<IndexViewModel> Blinds { get; set; } = new();

    public void OnGet()
    {
        var userId = this.User.GetUserId();

        var strat = Specifications.GetOwnedBlinds(userId);
        strat.FetchStrategy = Strategies.IncludeItemsAndVotesAndMembers();

        var blinds = this.blindRepository.FindAll(strat);

        this.Blinds = this.mapper.Map<List<IndexViewModel>>(blinds).OrderByDescending(x => x.Closed is not null).ToList();
    }
}
