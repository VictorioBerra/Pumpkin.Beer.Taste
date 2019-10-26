using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pumpkin.Beer.Taste.Services;

namespace Pumpkin.Beer.Taste.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IClockService clockService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DbSet<Blind> Blind { get; set; }

        public DbSet<BlindItem> BlindItem { get; set; }

        public DbSet<BlindVote> BlindVote { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    IClockService clockService,
                                    IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            this.clockService = clockService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override int SaveChanges()
        {
            AuditEntities();
            return base.SaveChanges();
        }

        private void AuditEntities()
        {
            // get entries that are being Added or Updated
            var modifiedEntries = ChangeTracker.Entries()
                    .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified));

            // Theres a good chance we are seeding here.
            if(httpContextAccessor.HttpContext == null)
            {
                return;
            }

            var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var now = clockService.UtcNow;

            foreach (var entry in modifiedEntries)
            {
                var entity = entry.Entity as AuditableEntity;

                // Identity stuff will never be a `AuditableEntity`
                if (entity == null)
                {
                    continue;
                }

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedByUserId = userId;
                    entity.CreatedDate = now;
                }

                entity.UpdatedByUserId = userId;
                entity.UpdatedDate = now;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var mysqlDateTimeOffsetToUTC = new ValueConverter<DateTimeOffset, DateTimeOffset>(
                v => v.UtcDateTime,
                v => v);

            modelBuilder
                .Entity<Blind>()
                .Property(e => e.Started)
                .HasConversion(mysqlDateTimeOffsetToUTC);

            modelBuilder
                .Entity<Blind>()
                .Property(e => e.Closed)
                .HasConversion(mysqlDateTimeOffsetToUTC);

            base.OnModelCreating(modelBuilder);
        }
    }
}
