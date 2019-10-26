using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Models;
using Pumpkin.Beer.Taste.Services;
using SharpRepository.Repository;
using SharpRepository.Repository.FetchStrategies;

namespace Pumpkin.Beer.Taste.Pages.VotePages
{
    public class IndexModel : PageModel
    {
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IClockService clockService;
        private readonly IRepository<Blind, int> blindRepository;
        private readonly IRepository<BlindItem, int> blindItemsRepository;

        public IndexModel(
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            IClockService clockService,
            IRepository<Blind, int> blindRepository,
            IRepository<BlindItem, int> blindItemsRepository)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.clockService = clockService;
            this.blindRepository = blindRepository;
            this.blindItemsRepository = blindItemsRepository;
        }

        [BindProperty]
        public BlindItemDto BlindItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var now = this.clockService.UtcNow;
            var userId = this.userManager.GetUserId(User);

            // Is open and not closed?
            var spec = Specifications.GetOpenBlinds(now)
                .And(x => x.Id == id)
                // Any items that have no votes, or have no votes of mine
                .And(x => x.BlindItems.Any(y => !y.BlindVotes.Any() || y.BlindVotes.Any(z => z.CreatedByUserId != userId)));

            var strat = new GenericFetchStrategy<Blind>();
            strat.Include(x => x.BlindItems.First().BlindVotes);

            spec.FetchStrategy = strat;

            var blind = blindRepository.Find(spec);
            if (blind == null)
            {
                return Page();
            }

            var firstItem = blind.BlindItems.OrderBy(x => x.ordinal).First();

            BlindItem = mapper.Map<BlindItemDto>(firstItem);

            return Page();

        }
    }
}
