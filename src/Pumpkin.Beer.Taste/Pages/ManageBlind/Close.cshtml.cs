namespace Pumpkin.Beer.Taste.Pages.BlindPages;

using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.Services;
using Pumpkin.Beer.Taste.ViewModels.ManageBlind;
using SharpRepository.Repository;

public class CloseModel : PageModel
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly IClockService clockService;
    private readonly IRepository<Blind, int> blindRepository;

    public CloseModel(
        ApplicationDbContext context,
        IMapper mapper,
        IClockService clockService,
        IRepository<Blind, int> blindRepository)
    {
        this.context = context;
        this.mapper = mapper;
        this.clockService = clockService;
        this.blindRepository = blindRepository;
    }

    [BindProperty]
    public CloseViewModel Blind { get; set; } = null!;

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        var blind = this.blindRepository.Get((int)id);
        if (blind == null)
        {
            return this.NotFound();
        }

        // Kick if its not theirs
        var userId = this.User.GetUserId();
        if (blind.CreatedByUserId != userId)
        {
            return this.Unauthorized();
        }

        this.Blind = this.mapper.Map<CloseViewModel>(blind);

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

        var now = this.clockService.Now;

        var blind = this.blindRepository.Get((int)id);
        if (blind == null)
        {
            return this.NotFound();
        }

        // Kick if its not theirs
        var userId = this.User.GetUserId();
        if (blind.CreatedByUserId != userId)
        {
            return this.Unauthorized();
        }

        this.Blind = this.mapper.Map<CloseViewModel>(blind);

        if (this.Blind != null)
        {
            blind.Closed = now.UtcDateTime;
            this.context.Attach(blind).State = EntityState.Modified;
            await this.context.SaveChangesAsync();
        }

        return this.RedirectToPage("./Index");
    }
}
