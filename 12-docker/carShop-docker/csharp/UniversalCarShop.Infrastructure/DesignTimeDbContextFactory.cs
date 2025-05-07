using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using UniversalCarShop.Infrastructure.Database;

namespace UniversalCarShop.Infrastructure;

internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql();

        return new AppDbContext(optionsBuilder.Options);
    }
}

