using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NewsAppBackend.Infrastructure.Database;

internal sealed class ReadWriteDbContextFactory : IDesignTimeDbContextFactory<ReadWriteDbContext>
{
    public ReadWriteDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ReadWriteDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=NewsAppBackend;Username=postgres;Password=postgres");

        return new ReadWriteDbContext(optionsBuilder.Options);
    }
}