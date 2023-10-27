namespace Pumpkin.Beer.Taste.Pages;

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
using Pumpkin.Beer.Taste.ViewModels.Home;
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

        this.ClosedBlinds = new List<IndexViewModel>();
    }

    [BindProperty]
    public List<IndexViewModel> Blinds { get; set; } = new();

    public List<IndexViewModel> ClosedBlinds { get; set; } = new();

    public void OnGet()
    {
        // Todo, maybe make two pages? One for authed and one they get bounced to if not authed?
        if (this.User.Identity?.IsAuthenticated ?? false)
        {
            var userId = this.User.GetUserId();

            var now = this.clockService.UtcNow;

            var spec = Specifications.GetOpenBlinds(now);
            spec.FetchStrategy = Strategies.IncludeItemsAndVotes();
            var blinds = this.blindRepository.FindAll(spec);

            this.Blinds = this.mapper.Map<List<IndexViewModel>>(blinds);

            this.ClosedBlinds = this.mapper.Map<List<IndexViewModel>>(this.blindRepository.FindAll(Specifications.GetClosedBlinds(now)));
        }
    }
}
