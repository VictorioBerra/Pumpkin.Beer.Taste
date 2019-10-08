using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pumpkin.Beer.Taste.Data.Entities;

namespace Pumpkin.Beer.Taste.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Beers> Beers { get; set; }

        public DbSet<Votes> Votes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
