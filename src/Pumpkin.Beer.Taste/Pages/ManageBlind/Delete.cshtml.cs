namespace Pumpkin.Beer.Taste.Pages.BlindPages;

using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.ViewModels.ManageBlind;
using SharpRepository.Repository;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly IRepository<Blind, int> blindRepository;

    public DeleteModel(
        ApplicationDbContext context,
        IMapper mapper,
        IRepository<Blind, int> blindRepository)
    {
        this.context = context;
        this.mapper = mapper;
        this.blindRepository = blindRepository;
    }

    [BindProperty]
    public DeleteViewModel Blind { get; set; } = null!;

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        // Kick if it has votes
        var spec = Specifications.GetBlindsWithNoVotes()
            .AndAlso(x => x.Id == id);
        var blind = this.blindRepository.Find(spec);
        if (blind == null)
        {
            this.ModelState.AddPageError("Voting has already started, you cannot delete this tasting.");
            return this.Page();
        }

        // Kick if its not theirs
        var userId = this.User.GetUserId();
        if (blind.CreatedByUserId != userId)
        {
            return this.Unauthorized();
        }

        this.Blind = this.mapper.Map<DeleteViewModel>(blind);

        if (this.Blind == null)
        {
            return this.NotFound();
        }

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        // Kick if it has votes
        var spec = Specifications.GetBlindsWithNoVotes()
            .AndAlso(x => x.Id == id);
        var blind = this.blindRepository.Find(spec);
        if (blind == null)
        {
            this.ModelState.AddPageError("Voting has already started, you cannot delete this tasting.");
            return this.Page();
        }

        // Kick if its not theirs
        var userId = this.User.GetUserId();
        if (blind.CreatedByUserId != userId)
        {
            return this.Unauthorized();
        }

        this.Blind = this.mapper.Map<DeleteViewModel>(blind);

        if (this.Blind != null)
        {
            this.context.Blind.Remove(blind);
            await this.context.SaveChangesAsync();
        }

        return this.RedirectToPage("./Index");
    }
}
