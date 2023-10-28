namespace Pumpkin.Beer.Taste.Pages;

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Eblindtensions;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.Services;
using Pumpkin.Beer.Taste.ViewModels.Home;
using SharpRepository.Repository;

public class IndexModel : PageModel
{
    private readonly IMapper mapper;
    private readonly IClockService clockService;
    private readonly IRepository<Blind, int> blindRepository;
    private readonly IRepository<UserInvite, int> inviteRepository;

    public IndexModel(
        IMapper mapper,
        IClockService clockService,
        IRepository<Blind, int> blindRepository,
        IRepository<UserInvite, int> inviteRepository)
    {
        this.mapper = mapper;
        this.clockService = clockService;
        this.blindRepository = blindRepository;
        this.ClosedBlinds = new List<IndexViewModel>();
        this.inviteRepository = inviteRepository;
        this.ClosedBlinds = new List<IndexViewModel>();
    }

    public List<IndexViewModel> Blinds { get; set; } = new();

    [Required]
    [BindProperty]
    [DisplayName("Invite Code")]
    public string? InviteCode { get; set; }

    public List<IndexViewModel> ClosedBlinds { get; set; } = new();

    public void OnGet()
    {
        // Todo, maybe make two pages? One for authed and one they get bounced to if not authed?
        if (this.User.Identity?.IsAuthenticated ?? false)
        {
            var now = this.clockService.UtcNow;

            var spec = Specifications.GetOpenBlinds(now);
            spec.FetchStrategy = Strategies.IncludeItemsAndVotes();
            var blinds = this.blindRepository.FindAll(spec);

            this.Blinds = this.mapper.Map<List<IndexViewModel>>(blinds);

            this.ClosedBlinds = this.mapper.Map<List<IndexViewModel>>(this.blindRepository.FindAll(Specifications.GetClosedBlinds(now)));
        }
    }

    public IActionResult OnPost()
    {
        if (this.User.Identity?.IsAuthenticated ?? false)
        {
            var userId = this.User.GetUserId();
            var now = this.clockService.UtcNow;

            var blindForInvite = this.blindRepository.Find(blind => blind.InviteCode == this.InviteCode);

            if (blindForInvite is null)
            {
                this.ModelState.AddPageError("Invalid invite code.");
                return this.Page();
            }

            var eblindistingLink = this.inviteRepository.Find(blind => blind.CreatedByUserId == userId && blind.BlindId == blindForInvite.Id);
            if (eblindistingLink is not null)
            {
                this.ModelState.AddPageError("You have already joined this tasting.");
                return this.Page();
            }

            var invite = new UserInvite
            {
                BlindId = blindForInvite.Id,
                CreatedByUserId = userId,
            };
            this.inviteRepository.Add(invite);

            if (blindForInvite.IsOpen(now))
            {
                return this.RedirectToPage("/BlindPages/Vote", new { id = blindForInvite.Id });
            }
            else
            {
                return this.RedirectToPage("/Index");
            }
        }

        return this.Page();
    }
}
