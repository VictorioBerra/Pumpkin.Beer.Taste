namespace Pumpkin.Beer.Taste.Pages;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Eblindtensions;
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
    IRepository<User, string> userRepository,
    IApplicationService applicationService) : PageModel
{
    public List<IndexViewModel> Blinds { get; set; } = [];

    [Required]
    [BindProperty]
    [DisplayName("Invite Code")]
    public string? InviteCode { get; set; }

    public DateTime CurrentUserLocalTimeAsUtcByProfileTimeZone { get; set; }

    public IActionResult OnGet()
    {
        var userId = this.User.GetUserId();
        var user = userRepository.Get(userId);
        var now = timeProvider.GetUtcNow();

        this.SetOpenBlinds(user, now);

        logger.LogInformation("Logged in user {UserId} loaded dashboard", userId);

        return this.Page();
    }

    public IActionResult OnPost()
    {
        var userId = this.User.GetUserId();
        var user = userRepository.Get(userId);
        var now = timeProvider.GetUtcNow();

        if (string.IsNullOrWhiteSpace(this.InviteCode))
        {
            this.ModelState.AddPageError("Missing code.");

            this.SetOpenBlinds(user, now);

            return this.Page();
        }

        var inviteAcceptResult = applicationService.AcceptInvite(this.User, this.InviteCode);

        if (inviteAcceptResult.Status is Ardalis.Result.ResultStatus.Error)
        {
            this.ModelState.AddPageError(inviteAcceptResult);

            this.SetOpenBlinds(user, now);

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

    private void SetOpenBlinds(
        User user,
        DateTimeOffset utcNow)
    {
        var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.WindowsTimeZoneId);

        this.CurrentUserLocalTimeAsUtcByProfileTimeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcNow, user.WindowsTimeZoneId).UtcDateTime;

        var spec = Specifications.GetOpenBlinds(this.CurrentUserLocalTimeAsUtcByProfileTimeZone, user.WindowsTimeZoneId)
            .AndAlso(Specifications.GetMemberOfBlinds(user.Id));
        spec.FetchStrategy = Strategies.IncludeItemsAndVotesAndMembers();

        this.Blinds = blindRepository.FindAll(spec)
            .Select(x => new IndexViewModel
            {
                Id = x.Id,
                Name = x.Name,
                CoverPhotoBase64 = x.CoverPhoto != null ? Convert.ToBase64String(x.CoverPhoto) : null,
                HasVotes = x.BlindItems.Any(x => x.BlindVotes.Count != 0),
                StartedUtc = x.StartedUtc,
                ClosedUtc = x.ClosedUtc,
                Started = TimeZoneInfo.ConvertTimeFromUtc(x.StartedUtc, userTimeZone),
                Closed = TimeZoneInfo.ConvertTimeFromUtc(x.ClosedUtc, userTimeZone),
                CreatedByUserDisplayName = x.CreatedByUserDisplayName,
                StartsInWindowsTimeZoneId = x.StartedWindowsTimeZoneId,
                CreatedByUserId = x.CreatedByUserId,
                NumMembers = x.UserInvites.Count,
            })
            .ToList();
    }
}
