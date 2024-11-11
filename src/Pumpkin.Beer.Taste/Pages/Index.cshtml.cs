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
using Pumpkin.Beer.Taste.ViewModels.Home;
using SharpRepository.Repository;
using SharpRepository.Repository.Specifications;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class IndexModel(
    ILogger<IndexModel> logger,
    IMapper mapper,
    TimeProvider timeProvider,
    IRepository<Blind, int> blindRepository,
    IRepository<UserInvite, int> inviteRepository) : PageModel
{
    public List<IndexViewModel> Blinds { get; set; } = [];

    [Required]
    [BindProperty]
    [DisplayName("Invite Code")]
    public string? InviteCode { get; set; }

    public void OnGet()
    {
        // Todo, maybe make two pages? One for authed and one they get bounced to if not authed?
        if (this.User.Identity?.IsAuthenticated ?? false)
        {
            var userId = this.User.GetUserId();
            var now = timeProvider.GetLocalNow();

            this.SetOpenBlinds(userId, now);

            logger.LogInformation("Logged in user {UserId} loaded dashboard", userId);
        }
    }

    public IActionResult OnPost()
    {
        if (this.User.Identity?.IsAuthenticated ?? false)
        {
            var userId = this.User.GetUserId();
            var now = timeProvider.GetLocalNow();

            var blindForInvite = blindRepository.Find(blind => blind.InviteCode == this.InviteCode);

            if (blindForInvite is null)
            {
                this.ModelState.AddPageError("Invalid invite code.");
                return this.Page();
            }

            var eblindistingLink = inviteRepository.Find(blind => blind.CreatedByUserId == userId && blind.BlindId == blindForInvite.Id);
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
            inviteRepository.Add(invite);

            if (blindForInvite.IsOpen(now))
            {
                return this.RedirectToPage("/Vote/Index", new { id = blindForInvite.Id });
            }
            else
            {
                return this.RedirectToPage("/Index");
            }
        }

        return this.Page();
    }

    private void SetOpenBlinds(string userId, DateTimeOffset now)
    {
        var spec = Specifications.GetOpenBlinds(now).AndAlso(Specifications.GetMemberOfBlinds(userId));
        spec.FetchStrategy = Strategies.IncludeItemsAndVotesAndMembers();
        var openBlinds = blindRepository.FindAll(spec);

        this.Blinds = mapper.Map<List<IndexViewModel>>(openBlinds);
    }
}
