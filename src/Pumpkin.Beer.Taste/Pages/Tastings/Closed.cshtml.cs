namespace Pumpkin.Beer.Taste.Pages.Tastings;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.ViewModels.Tastings;
using SharpRepository.Repository;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class ClosedModel(
    IMapper mapper,
    TimeProvider timeProvider,
    IRepository<Blind, int> blindRepository) : PageModel
{
    public List<IndexViewModel> ClosedBlinds { get; set; } = [];

    public void OnGet()
    {
        var userId = this.User.GetUserId();
        var now = timeProvider.GetLocalNow();

        this.SetClosedBlinds(userId, now);
    }

    private void SetClosedBlinds(string userId, DateTimeOffset now)
    {
        var spec = Specifications.GetClosedBlinds(now).AndAlso(Specifications.GetMemberOfBlinds(userId));
        spec.FetchStrategy = Strategies.IncludeItemsAndVotesAndMembers();
        var openBlinds = blindRepository.FindAll(spec);

        this.ClosedBlinds = mapper.Map<List<IndexViewModel>>(openBlinds);
    }
}
