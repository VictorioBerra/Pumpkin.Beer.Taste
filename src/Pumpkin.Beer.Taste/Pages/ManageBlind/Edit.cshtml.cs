﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Models;
using SharpRepository.Repository;

namespace Pumpkin.Beer.Taste.Pages.BlindPages
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IRepository<Blind, int> blindRepository;
        private readonly UserManager<IdentityUser> userManager;

        public EditModel(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IMapper mapper,
            IRepository<Blind, int> blindRepository)
        {
            this.userManager = userManager;
            this.context = context;
            this.mapper = mapper;
            this.blindRepository = blindRepository;
        }

        [BindProperty]
        public BlindDto Blind { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Kick if it has votes
            var spec = Specifications.GetBlindsWithNoVotes()
                .And(x => x.Id == id);
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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kick if it has votes
            var spec = Specifications.GetBlindsWithNoVotes()
                .And(x => x.Id == id);
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

            var blindToEdit = mapper.Map<Blind>(Blind);

            context.Attach(blindToEdit).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
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
            return context.Blind.Any(e => e.Id == id);
        }
    }
}
