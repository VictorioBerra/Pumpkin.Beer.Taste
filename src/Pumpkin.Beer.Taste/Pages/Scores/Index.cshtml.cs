using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Models;
using Pumpkin.Beer.Taste.Services;
using SharpRepository.Repository;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Specifications;

namespace Pumpkin.Beer.Taste.Pages.ScorePages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMapper mapper;
        private readonly IClockService clockService;
        private readonly IRepository<Blind, int> blindRepository;
        private readonly IRepository<BlindItem, int> blindItemRepository;
        private readonly UserManager<IdentityUser> userManager;


        public List<BlindItemScoresDto> BlindItemScore { get; set; }

        public BlindDto Blind { get; set; }

        public IndexModel(
            ILogger<IndexModel> logger,
            IMapper mapper,
            IClockService clockService,
            IRepository<Blind, int> blindRepository,
            IRepository<BlindItem, int> blindItemRepository,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            this.mapper = mapper;
            this.clockService = clockService;
            this.blindRepository = blindRepository;
            this.blindItemRepository = blindItemRepository;
            this.userManager = userManager;

            BlindItemScore = new List<BlindItemScoresDto>();
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var now = this.clockService.UtcNow;

            var spec = Specifications
                .GetClosedBlinds(now)
                .AndAlso(x => x.Id == id);
            spec.FetchStrategy = Strategies.IncludeItemsAndVotes();
            var blind = blindRepository.Find(spec);
            if (blind == null)
            {
                // TODO better error??
                return NotFound();
            }

            if (!blind.BlindItems.Any(x => x.BlindVotes.Any()))
            {
                // TODO better error??
                return NotFound();
            }

            Blind = mapper.Map<BlindDto>(blind);
            BlindItemScore = mapper.Map<List<BlindItemScoresDto>>(blind.BlindItems).OrderByDescending(x => x.TotalScore).ToList();

            return Page();
        }
    }
}
