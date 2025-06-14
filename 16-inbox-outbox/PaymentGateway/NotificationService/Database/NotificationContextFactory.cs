using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NotificationService.Database;

internal sealed class NotificationContextFactory : IDesignTimeDbContextFactory<NotificationContext>
{
    public NotificationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NotificationContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=NotificationService;Username=postgres;Password=postgres");

        return new NotificationContext(optionsBuilder.Options);
    }
}