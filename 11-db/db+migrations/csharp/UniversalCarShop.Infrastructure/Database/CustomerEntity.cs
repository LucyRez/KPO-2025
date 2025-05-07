namespace UniversalCarShop.Infrastructure.Database;

internal sealed record CustomerEntity(string Name, int LegPower, int HandPower, int? CarNumber)
{
    public int? CarNumber { get; set; } = CarNumber;
    public CarEntity? Car { get; set; }
}

