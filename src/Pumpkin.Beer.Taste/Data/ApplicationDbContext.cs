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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blind>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Blind>()
                .Property(b => b.Created)
                .HasDefaultValue(DateTime.Now);

            base.OnModelCreating(modelBuilder);

        }
    }
}
