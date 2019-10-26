using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Pumpkin.Beer.Taste.Pages.BlindPages
{
    public class CloseModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IClockService clockService;
        private readonly IRepository<Blind, int> blindRepository;
        private readonly UserManager<IdentityUser> userManager;

        public CloseModel(
            ApplicationDbContext context,
            IMapper mapper,
            IClockService clockService,
            IRepository<Blind, int> blindRepository,
            UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.clockService = clockService;
            this.blindRepository = blindRepository;
            this.userManager = userManager;
        }

        [BindProperty]
        public BlindDto Blind { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Kick if it has votes
            var spec = Specifications.GetBlindsWithNoVotes()
                .AndAlso(x => x.Id == id);
            var blind = blindRepository.Find(spec);
            if (blind == null)
            {
                // TODO better error??
                return NotFound();
            }

            // Kick if its not theirs
            var userId = this.userManager.GetUserId(User);
            if (blind.CreatedByUserId != userId)
            {
                return Unauthorized();
            }

            Blind = mapper.Map<BlindDto>(blind);

            if (Blind == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var now = this.clockService.UtcNow;

            // Kick if it has votes
            var spec = Specifications.GetBlindsWithNoVotes()
                .AndAlso(x => x.Id == id);
            var blind = blindRepository.Find(spec);
            if (blind == null)
            {
                // TODO better error??
                return NotFound();
            }

            // Kick if its not theirs
            var userId = this.userManager.GetUserId(User);
            if (blind.CreatedByUserId != userId)
            {
                return Unauthorized();
            }

            Blind = mapper.Map<BlindDto>(blind);

            if (Blind != null)
            {
                blind.Closed = now;
                context.Attach(blind).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
