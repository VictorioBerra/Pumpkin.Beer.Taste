namespace Pumpkin.Beer.Taste.Data;

using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class MyKeysContext(DbContextOptions<MyKeysContext> options) : DbContext(options), IDataProtectionKeyContext
{
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set the default schema
        modelBuilder.HasDefaultSchema("dp");
    }
}
