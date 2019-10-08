using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Pumpkin.Beer.Taste.Data.Entities
{
    public class Votes
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }


    }
}
