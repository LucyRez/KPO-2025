using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace UniversalCarShop.Infrastructure.Database;

internal sealed class AppDbContext : DbContext
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new();

    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<CarEntity> Cars { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerEntity>(e =>
        {
            e.HasKey(c => c.Name);

            e.HasOne(c => c.Car)
                .WithOne()
                .HasForeignKey<CustomerEntity>(c => c.CarNumber);
        });

        modelBuilder.Entity<CarEntity>(e =>
        {
            e.HasKey(c => c.Number);

            e.Property(c => c.Engine)
                .HasConversion(
                    engine => JsonSerializer.Serialize<EngineEntityBase>(engine, _jsonSerializerOptions),
                    json => JsonSerializer.Deserialize<EngineEntityBase>(json, _jsonSerializerOptions)!
                );
        });
    }
}

