namespace UniversalCarShop.UseCases.DTOs;

public sealed record CustomerDto(
    string Name,
    int LegPower,
    int HandPower,
    CarDto? Car
);
