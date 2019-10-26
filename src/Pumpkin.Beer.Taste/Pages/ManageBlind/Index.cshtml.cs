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
using SharpRepository.Repository;

namespace Pumpkin.Beer.Taste.Pages.BlindPages
{
    public class IndexModel : PageModel
    {
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IRepository<Blind, int> blindRepository;

        public IndexModel(
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            IRepository<Blind, int> blindRepository)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.blindRepository = blindRepository;
        }

        public IList<BlindDto> Blinds { get;set; }

        public void OnGet()
        {
            var userId = userManager.GetUserId(User);

            var blinds = blindRepository.FindAll(Specifications.GetOnlyMyBlinds(userId));

            Blinds = mapper.Map<List<BlindDto>>(blinds);
        }
    }
}
