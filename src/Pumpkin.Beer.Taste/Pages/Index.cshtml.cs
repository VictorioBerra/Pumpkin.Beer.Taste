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

        [BindProperty]
        public IList<BlindDto> Blinds { get; set; }

        public IndexModel(
            ILogger<IndexModel> logger,
            IMapper mapper,
            IClockService clockService,
            IRepository<Blind, int> blindRepository)
        {
            _logger = logger;
            this.mapper = mapper;
            this.clockService = clockService;
            this.blindRepository = blindRepository;
        }

        public void OnGet()
        {
            // Todo, maybe make two pages? One for authed and one they get bounced to if not authed?
            if (User.Identity.IsAuthenticated)
            {
                var now = this.clockService.UtcNow;

                var blinds = blindRepository.FindAll(Specifications.GetOpenBlinds(now));

                Blinds = this.mapper.Map<List<BlindDto>>(blinds);
            }
        }
    }
}
