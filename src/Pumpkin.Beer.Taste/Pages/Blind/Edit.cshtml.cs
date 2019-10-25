using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Models;

namespace Pumpkin.Beer.Taste.Pages.BlindPages
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public EditModel(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        [BindProperty]
        public BlindDto Blind { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blind = await _context.Blind.FirstOrDefaultAsync(m => m.Id == id);
            Blind = mapper.Map<BlindDto>(blind);

            if (Blind == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // TODO
            // Dont allow edits if votes have been placed already!

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var blind = mapper.Map<Blind>(Blind);

            _context.Attach(blind).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlindExists(Blind.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BlindExists(int id)
        {
            return _context.Blind.Any(e => e.Id == id);
        }
    }
}
