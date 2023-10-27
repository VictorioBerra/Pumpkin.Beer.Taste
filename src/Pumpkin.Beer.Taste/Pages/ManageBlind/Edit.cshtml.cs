namespace Pumpkin.Beer.Taste.Pages.BlindPages;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.ViewModels.ManageBlind;
using SharpRepository.Repository;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly IRepository<Blind, int> blindRepository;
    private readonly IRepository<BlindItem, int> blindItemRepository;

    public EditModel(
        ApplicationDbContext context,
        IMapper mapper,
        IRepository<Blind, int> blindRepository,
        IRepository<BlindItem, int> blindItemRepository)
    {
        this.context = context;
        this.mapper = mapper;
        this.blindRepository = blindRepository;
        this.blindItemRepository = blindItemRepository;
    }

    [BindProperty]
    public EditViewModel Blind { get; set; } = null!;

    [BindProperty]
    public List<EditItemViewModel> BlindItems { get; set; } = new();

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        // Kick if it has votes
        var spec = Specifications
            .GetBlindsWithNoVotes()
            .AndAlso(x => x.Id == id);
        var blind = this.blindRepository.Find(spec);
        if (blind == null)
        {
            this.ModelState.AddPageError("Voting has already started, you cannot edit this tasting.");
            return this.Page();
        }

        // Kick if its not theirs
        var userId = this.User.GetUserId();
        if (blind.CreatedByUserId != userId)
        {
            return this.Unauthorized();
        }

        this.Blind = this.mapper.Map<EditViewModel>(blind);
        this.BlindItems = this.mapper.Map<List<EditItemViewModel>>(this.blindItemRepository.FindAll(x => x.BlindId == id));

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return this.NotFound();
        }

        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        // Kick if it has votes
        var spec = Specifications.GetBlindsWithNoVotes()
            .AndAlso(x => x.Id == id);
        var blind = this.blindRepository.Find(spec);
        if (blind == null)
        {
            this.ModelState.AddPageError("Voting has already started, you cannot edit this tasting.");
            return this.Page();
        }

        // Kick if its not theirs
        var userId = this.User.GetUserId();
        if (blind.CreatedByUserId != userId)
        {
            return this.Unauthorized();
        }

        var blindToEdit = this.mapper.Map<Blind>(this.Blind);
        blindToEdit.Id = id.Value;

        var blindItems = blind.BlindItems.ToList();
        for (var i = 0; i < blindItems.Count; i++)
        {
            blindItems[i].Ordinal = i;
        }

        blind.BlindItems = blindItems;

        this.context.Attach(blindToEdit).State = EntityState.Modified;

        try
        {
            await this.context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!this.BlindExists(this.Blind.Id))
            {
                return this.NotFound();
            }
            else
            {
                throw;
            }
        }

        return this.RedirectToPage("./Index");
    }

    private bool BlindExists(int id) => this.context.Blind.Any(e => e.Id == id);
}
