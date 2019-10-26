using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Pumpkin.Beer.Taste.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMapper mapper;
        private readonly IClockService clockService;
        private readonly IRepository<Blind, int> blindRepository;
        private readonly IRepository<BlindItem, int> blindItemRepository;
        private readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public IList<BlindDto> Blinds { get; set; }

        public IList<BlindDto> ClosedBlinds { get; set; }

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

            ClosedBlinds = new List<BlindDto>();
        }

        public void OnGet()
        {
            // Todo, maybe make two pages? One for authed and one they get bounced to if not authed?
            if (User.Identity.IsAuthenticated)
            {
                var userId = userManager.GetUserId(User);

                var now = this.clockService.UtcNow;

                var spec = Specifications.GetOpenBlinds(now);
                spec.FetchStrategy = new GenericFetchStrategy<Blind>().Include(x => x.BlindItems);
                var blinds = blindRepository.FindAll(spec);

                // TODO: Yes I know this is a nightmare, but MySQL was throwing a fit when I tried to do a complex .And
                var openUnvotedBlinds = new List<Blind>();
                foreach (var blind in blinds)
                {
                    var itemSpec = Specifications.GetBlindsWithItemsWithVotesOfMine(userId)
                        .AndAlso(x => x.BlindId == blind.Id);

                    var foundItemWithNoVote = false;
                    foreach (var item in blind.BlindItems)
                    {
                        foundItemWithNoVote = !blindItemRepository.FindAll(itemSpec.And(x => x.Id == item.Id)).Any();
                    }

                    if (foundItemWithNoVote)
                    {
                        openUnvotedBlinds.Add(blind);
                    }
                }

                Blinds = this.mapper.Map<List<BlindDto>>(openUnvotedBlinds);

                ClosedBlinds = this.mapper.Map<List<BlindDto>>(blindRepository.FindAll(Specifications.GetClosedBlinds(now)));

            }
        }
    }
}
