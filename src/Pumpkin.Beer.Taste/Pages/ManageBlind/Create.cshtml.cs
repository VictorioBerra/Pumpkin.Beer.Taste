using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Models;
using SharpRepository.Repository;

namespace Pumpkin.Beer.Taste.Pages.BlindPages
{
    public class CreateModel : PageModel
    {
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IRepository<Blind, int> blindRepository;

        public CreateModel(
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            IRepository<Blind, int> blindRepository)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.blindRepository = blindRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public BlindDto Blind { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var blind = mapper.Map<Blind>(Blind);
            blind.CreatedByUserId = this.userManager.GetUserId(User);

            var BlindItems = blind.BlindItems.ToList();
            for (int i = 0; i < BlindItems.Count; i++)
            {
                BlindItems[i].ordinal = i;
            }
            blind.BlindItems = BlindItems;

            blindRepository.Add(blind);

            return RedirectToPage("./Index");
        }
    }
}
