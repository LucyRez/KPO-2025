using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using UniversalCarShop.Infrastructure.Database;

namespace UniversalCarShop.Infrastructure;

internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=:memory:");

        return new AppDbContext(optionsBuilder.Options);
    }
}

