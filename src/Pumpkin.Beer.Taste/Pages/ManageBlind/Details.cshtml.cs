using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Models;

namespace Pumpkin.Beer.Taste.Pages.BlindPages
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public DetailsModel(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

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
    }
}
