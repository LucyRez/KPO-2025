using System;

namespace UniversalCarShop.UseCases.Engines;

/// <summary>
/// Класс для хранения информации о параметрах двигателя
/// </summary>
public record PedalEngineParams (
    int PedalSize // У класса есть всего одно свойство - для хранения размера педалей
);
