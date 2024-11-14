namespace Pumpkin.Beer.Taste.Pages.BlindPages;

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Eblindtensions;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.ViewModels.ManageBlind;
using QRCoder;
using SharpRepository.Repository;
using SharpRepository.Repository.Queries;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class IndexModel(
    TimeProvider timeProvider,
    IRepository<Blind, int> blindRepository) : PageModel
{
    public List<IndexViewModel> Blinds { get; set; } = [];

    public void OnGet()
    {
        var userId = this.User.GetUserId();
        var now = timeProvider.GetLocalNow();

        var strat = Specifications.GetOwnedBlinds(userId);
        strat.FetchStrategy = Strategies.IncludeItemsAndVotesAndMembers();

        var sortingOptions = new SortingOptions<Blind, DateTimeOffset?>(x => x.Started, isDescending: true);

        this.Blinds = [.. blindRepository.FindAll(
            strat,
            x => new IndexViewModel
            {
                Id = x.Id,
                Name = x.Name,
                CoverPhotoBase64 = x.CoverPhoto != null ? Convert.ToBase64String(x.CoverPhoto) : null,
                InviteCode = x.InviteCode,
                NumMembers = x.UserInvites.Count,
                NumItems = x.BlindItems.Count,
                NumVotes = x.BlindItems.SelectMany(x => x.BlindVotes).Count(),
                IsOpen = x.IsOpen(now),
                Started = x.Started,
                Closed = x.Closed,
                CreatedByUserDisplayName = x.CreatedByUserDisplayName,
            },
            sortingOptions)];

        foreach (var blind in this.Blinds)
        {
            var link = this.JoinAndVoteLink(blind.Id, blind.InviteCode);
            blind.JoinAndVoteLink = link;
            blind.QRCodeBase64 = GenerateQRCode(link);
        }
    }

    private static string GenerateQRCode(string joinAndVoteLink)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(joinAndVoteLink, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new Base64QRCode(qrCodeData);
        var qrCodeImage = qrCode.GetGraphic(20);

        return qrCodeImage;
    }

    private string JoinAndVoteLink(int blindId, string inviteCode)
    {
        var request = this.HttpContext.Request;
        var linkToVoteWithInvite = this.Url.Page(
            "/Vote/Index",
            null,
            new { id = blindId, inviteCode },
            request.Scheme,
            request.Host.ToString())!;

        return linkToVoteWithInvite.ToString();
    }
}
