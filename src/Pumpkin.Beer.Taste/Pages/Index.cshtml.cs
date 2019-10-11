using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Models;

namespace Pumpkin.Beer.Taste.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;

        [BindProperty]
        public IList<BlindDto> Blinds { get; set; }

        public IndexModel(
            ILogger<IndexModel> logger,
            IMapper mapper,
            ApplicationDbContext context)
        {
            _logger = logger;
            this.mapper = mapper;
            this.context = context;
        }

        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                var blinds = context
                    .Blind
                    .AsNoTracking()
                    .Where(x => x.Started != null && x.Closed == null && x.Started < DateTime.Now)
                    //.ProjectTo<BlindDto>()
                    .ToList();

                Blinds = this.mapper.Map<List<BlindDto>>(blinds);
            }
        }
    }
}
