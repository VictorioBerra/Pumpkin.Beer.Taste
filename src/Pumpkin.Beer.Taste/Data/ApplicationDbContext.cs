using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Pumpkin.Beer.Taste.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Blind> Blind { get; set; }

        public DbSet<BlindItem> BlindItem { get; set; }

        public DbSet<BlindVote> BlindVote { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
