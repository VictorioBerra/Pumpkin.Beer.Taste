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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public IndexModel(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public IList<BlindDto> Blinds { get;set; }

        public async Task OnGetAsync()
        {
            var blinds = await _context
                .Blind
                .AsNoTracking()
                .ToListAsync();

            Blinds = mapper.Map<List<BlindDto>>(blinds);
        }
    }
}
