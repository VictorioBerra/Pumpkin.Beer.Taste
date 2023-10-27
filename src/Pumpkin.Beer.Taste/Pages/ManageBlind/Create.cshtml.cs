namespace Pumpkin.Beer.Taste.Pages.BlindPages;

using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.ViewModels.ManageBlind;
using SharpRepository.Repository;

public class CreateModel : PageModel
{
    private readonly IMapper mapper;
    private readonly IRepository<Blind, int> blindRepository;

    public CreateModel(
        IMapper mapper,
        IRepository<Blind, int> blindRepository)
    {
        this.mapper = mapper;
        this.blindRepository = blindRepository;
    }

    [BindProperty]
    public CreateViewModel Blind { get; set; } = null!;

    public IActionResult OnGet() => this.Page();

    public IActionResult OnPost()
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        var blind = this.mapper.Map<Blind>(this.Blind);
        blind.CreatedByUserId = this.User.GetUserId();

        var blindItems = blind.BlindItems.ToList();
        for (var i = 0; i < blindItems.Count; i++)
        {
            blindItems[i].Ordinal = i;
        }

        blind.BlindItems = blindItems;

        this.blindRepository.Add(blind);

        return this.RedirectToPage("./Index");
    }
}
