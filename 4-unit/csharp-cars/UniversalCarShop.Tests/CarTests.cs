using Xunit;
using UniversalCarShop;
using NSubstitute;

namespace UniversalCarShop.Tests;

public class CarTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsCompatible_Call_ShouldReturnExpectedResult(bool isCompatible) // Проверяем, что метод IsCompatible возвращает ожидаемый результат
    {
        // Arrange
        var engine = Substitute.For<IEngine>(); // Создаем мок объекта класса IEngine
        engine.IsCompatible(Arg.Any<Customer>()).Returns(isCompatible); // Устанавливаем, что метод IsCompatible будет возвращать значение из переменной isCompatible

        var car = new Car(engine, 1); // Создаем объект класса Car с моком двигателя и номером 1
        var customer = new Customer("Test", legPower: 0, handPower: 0); // Создаем покупателя с нулевой силой ног и рук

        // Act & Assert
        Assert.Equal(isCompatible, car.IsCompatible(customer)); // Проверяем, что метод IsCompatible возвращает значение из переменной isCompatible
    }
}