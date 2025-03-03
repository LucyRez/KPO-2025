using Xunit;
using UniversalCarShop;

namespace UniversalCarShop.Tests;

public class PedalEngineTests
{
    [Fact]
    public void Size_Read_ShouldReturnExpectedValue() // Проверяем, что свойство Size возвращает ожидаемое значение
    {
        // Arrange
        var engine = new PedalEngine(10); // Создаем объект класса PedalEngine с размером педали 10

        // Act & Assert
        Assert.Equal(10, engine.Size);
    }

    [Theory]
    [InlineData(6, true)]
    [InlineData(5, false)]
    [InlineData(4, false)]
    public void IsCompatible_Call_ShouldReturnExpectedResult(int legPower, bool expectedResult) // Проверяем, что метод IsCompatible возвращает ожидаемый результат
    {
        // Arrange
        var engine = new PedalEngine(10); // Создаем объект класса PedalEngine с размером педали 10
        var customer = new Customer("Test", legPower, handPower: 10); // Создаем покупателя с заданной силой ног и фиксированной силой рук

        // Act & Assert
        Assert.Equal(expectedResult, engine.IsCompatible(customer)); // Проверяем, что метод IsCompatible возвращает ожидаемый результат
    }
} 