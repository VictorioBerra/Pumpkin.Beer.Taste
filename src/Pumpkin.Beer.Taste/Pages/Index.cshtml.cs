using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Pumpkin.Beer.Taste.Data;
using SharpRepository.Repository;

namespace Pumpkin.Beer.Taste.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRepository<Blind, int> blindRepository;

        public List<Blind> Blinds { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IRepository<Blind, int> BlindRepository)
        {
            _logger = logger;
            blindRepository = BlindRepository;
        }

        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                // TODO make this an extension? GetAllStarted
                Blinds = blindRepository.FindAll(x => x.Closed == null && x.Started != null).ToList();
            }
        }
    }
}
