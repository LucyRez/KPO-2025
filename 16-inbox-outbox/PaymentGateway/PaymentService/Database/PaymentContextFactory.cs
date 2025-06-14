using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PaymentService.Database;

internal sealed class PaymentContextFactory : IDesignTimeDbContextFactory<PaymentContext>
{
    public PaymentContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PaymentContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=PaymentGateway;Username=postgres;Password=postgres");

        return new PaymentContext(optionsBuilder.Options);
    }
}