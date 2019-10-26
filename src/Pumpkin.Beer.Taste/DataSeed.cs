using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste
{
    public class DataSeed
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IClockService clockService;
        private readonly ILogger logger;

        public DataSeed(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            ILoggerFactory loggerFactory,
            IClockService clockService)
        {
            this.context = context;
            this.userManager = userManager;
            this.clockService = clockService;
            this.logger = loggerFactory.CreateLogger<DataSeed>();
        }

        //public async Task Seed()
        //{
            // TODO, It is not possible to do this without an HTTP Context

            //var adminName = Constants.InitialAdminName;

            //var admin = await userManager.FindByNameAsync(adminName);

            //if(!context.Blind.Any())
            //{
            //    var newBlind = new Blind 
            //    {
            //        Name = "Blind 1",
            //        Started = clockService.UtcNow.AddMinutes(-1), // Opened a minute ago.
            //        CreatedByUserId = admin.Id,
            //        BlindItems = new List<BlindItem> 
            //        { 
            //            new BlindItem() 
            //            {
            //                ordinal = 0,
            //                Name = "Drink 0"
            //            },
            //            new BlindItem()
            //            {
            //                ordinal = 2,
            //                Name = "Drink 2"
            //            },
            //            new BlindItem()
            //            {
            //                ordinal = 1,
            //                Name = "Drink 1"
            //            }
            //        }
            //    };

            //    context.Blind.Add(newBlind);

            //    await context.SaveChangesAsync();
            //}
        //}
    }
}
