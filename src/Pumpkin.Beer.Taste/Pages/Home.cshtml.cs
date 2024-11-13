namespace Pumpkin.Beer.Taste.Pages;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.Services;
using Pumpkin.Beer.Taste.ViewModels.Home;
using SharpRepository.Repository;

[Authorize]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class HomeModel(
    ILogger<IndexModel> logger,
    TimeProvider timeProvider,
    IRepository<Blind, int> blindRepository,
    IApplicationService applicationService) : PageModel
{
    public List<IndexViewModel> Blinds { get; set; } = [];

    [Required]
    [BindProperty]
    [DisplayName("Invite Code")]
    public string? InviteCode { get; set; }

    public IActionResult OnGet()
    {
        var userId = this.User.GetUserId();
        var now = timeProvider.GetLocalNow();

        this.SetOpenBlinds(userId, now);

        logger.LogInformation("Logged in user {UserId} loaded dashboard", userId);

        return this.Page();
    }

    public IActionResult OnPost()
    {
        Guard.Against.NullOrWhiteSpace(this.InviteCode, nameof(this.InviteCode));

        // Cant put [Authorize] attribute on Razor Page handlers, so check here.
        if (this.User.Identity?.IsAuthenticated ?? false)
        {
            var inviteAcceptResult = applicationService.AcceptInvite(this.User, this.InviteCode);

            if (inviteAcceptResult.Status is Ardalis.Result.ResultStatus.Error)
            {
                this.ModelState.AddPageError(inviteAcceptResult);
                return this.Page();
            }

            // No blind means it was probably closed, send to see results.
            if (inviteAcceptResult.Value is null)
            {
                return this.RedirectToPage("/Index");
            }
            else
            {
                return this.RedirectToPage("/Vote/Index", new { id = inviteAcceptResult.Value.Id });
            }
        }

        return this.Page();
    }

    private void SetOpenBlinds(string userId, DateTimeOffset now)
    {
        var spec = Specifications.GetOpenBlinds(now).AndAlso(Specifications.GetMemberOfBlinds(userId));
        spec.FetchStrategy = Strategies.IncludeItemsAndVotesAndMembers();
        this.Blinds = blindRepository.FindAll(spec, x => new IndexViewModel
        {
            Id = x.Id,
            Name = x.Name,
            HasVotes = x.BlindItems.Any(x => x.BlindVotes.Count != 0),
            Started = x.Started,
            Closed = x.Closed,
            CreatedByUserDisplayName = x.CreatedByUserDisplayName,
            CreatedByUserId = x.CreatedByUserId,
            NumMembers = x.UserInvites.Count,
        }).ToList();
    }
}
