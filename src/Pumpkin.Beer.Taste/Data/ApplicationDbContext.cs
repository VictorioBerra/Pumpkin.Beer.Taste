namespace Pumpkin.Beer.Taste.Data;

using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Pumpkin.Beer.Taste.Extensions;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    TimeProvider timeProvider,
    IHttpContextAccessor httpContextAccessor) : DbContext(options)
{
    public DbSet<Blind> Blind { get; set; }

    public DbSet<UserInvite> UserInvite { get; set; }

    public DbSet<BlindItem> BlindItem { get; set; }

    public DbSet<BlindVote> BlindVote { get; set; }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        this.AuditEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.AuditEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.AuditEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override int SaveChanges()
    {
        this.AuditEntities();
        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set the default schema
        modelBuilder.HasDefaultSchema("app");
    }

    private void AuditEntities()
    {
        // get entries that are being Added or Updated
        var modifiedEntries = this.ChangeTracker.Entries()
                .Where(x => x.State is EntityState.Added or EntityState.Modified);

        // Theres a good chance we are seeding here.
        if (httpContextAccessor.HttpContext == null)
        {
            return;
        }

        var userId = httpContextAccessor.HttpContext.User.GetUserId();
        var username = httpContextAccessor.HttpContext.User.GetUsername();

        Guard.Against.Null(userId);

        var now = timeProvider.GetLocalNow();

        foreach (var entry in modifiedEntries)
        {
            if (entry.Entity is not AuditableEntity entity)
            {
                continue;
            }

            if (entry.State == EntityState.Added)
            {
                entity.CreatedByUserId = userId;
                entity.CreatedByUserDisplayName = username;
                entity.CreatedDate = now;
            }
            else
            {
                entry.Property(nameof(AuditableEntity.CreatedByUserId)).IsModified = false;
                entry.Property(nameof(AuditableEntity.CreatedByUserDisplayName)).IsModified = false;
                entry.Property(nameof(AuditableEntity.CreatedDate)).IsModified = false;
            }

            entity.UpdatedByUserId = userId;
            entity.UpdatedByUserDisplayName = username;
            entity.UpdatedDate = now;
        }
    }
}
