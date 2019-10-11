using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Models;

namespace Pumpkin.Beer.Taste.Pages.BlindPages
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public CreateModel(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public BlindDto Blind { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var blind = mapper.Map<Blind>(Blind);

            _context.Blind.Add(blind);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
