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
        private readonly IRepository<BlindVote, int> blindVoteRepository;
        private readonly IRepository<BlindItem, int> blindItemRepository;

        public IndexModel(
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            IClockService clockService,
            IRepository<Blind, int> blindRepository,
            IRepository<BlindVote, int> blindVoteRepository,
            IRepository<BlindItem, int> blindItemRepository)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.clockService = clockService;
            this.blindRepository = blindRepository;
            this.blindVoteRepository = blindVoteRepository;
            this.blindItemRepository = blindItemRepository;
        }

        [BindProperty]
        public BlindVoteDto BlindVote { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var now = this.clockService.UtcNow;
            var userId = this.userManager.GetUserId(User);

            // Is open and not closed?
            var spec = Specifications.GetOpenBlinds(now)
                .AndAlso(x => x.Id == id);

            var strat = new GenericFetchStrategy<Blind>();
            strat.Include(x => x.BlindItems.First().BlindVotes);

            spec.FetchStrategy = strat;

            var blind = blindRepository.Find(spec);
            if (blind == null)
            {
                return Page();
            }

            var blindItemSpec = Specifications.GetBlindsWithItemsWithNoVotesOfMine(userId)
                .AndAlso(x => x.BlindId == id);
            blindItemSpec.FetchStrategy = Strategies.IncludeBlindAndVotes();

            var firstItem = blindItemRepository
                .FindAll(blindItemSpec)
                .OrderBy(x => x.ordinal)
                .FirstOrDefault();

            if(firstItem == null)
            {
                return Page();
            }

            BlindVote = new BlindVoteDto()
            {
                BlindItem = mapper.Map<BlindItemDto>(firstItem),
                BlindItemId = firstItem.Id
            };

            return Page();

        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }

            // TODO, Perform the same checks as above.

            var now = this.clockService.UtcNow;
            var userId = this.userManager.GetUserId(User);

            // Is open and not closed?
            var spec = Specifications.GetOpenBlinds(now)
                .AndAlso(x => x.Id == id);

            var strat = Strategies.IncludeItemsAndVotes();

            spec.FetchStrategy = strat;

            var blind = blindRepository.Find(spec);
            if (blind == null)
            {
                return Page();
            }

            var newVote = mapper.Map<BlindVote>(BlindVote);

            blindVoteRepository.Add(newVote);

            // Redirect to myself
            return RedirectToPage("./Index", new { Id = id });
        }
    }
}
