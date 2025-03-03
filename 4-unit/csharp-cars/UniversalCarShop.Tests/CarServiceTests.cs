using Xunit;
using UniversalCarShop;
using NSubstitute;

namespace UniversalCarShop.Tests;

public class CarServiceTests
{
    [Fact]
    public void AddCar_Call_NumberShouldBeUnique() // Проверяем, что номера автомобилей уникальны
    {
        // Arrange
        var carService = new CarService(); // Создаем объект класса CarService
        var carFactory = Substitute.For<ICarFactory<int>>(); // Создаем мок фабрики автомобилей
        var carParams = 1; // Создаем параметры для автомобиля

        // Act
        carService.AddCar(carFactory, carParams); // Добавляем автомобиль в список
        carService.AddCar(carFactory, carParams); // Добавляем второй автомобиль в список

        // Assert
        carFactory.Received(1).CreateCar(carParams, 1); // Проверяем, что метод CreateCar был вызван с параметрами carParams и номером 1
        carFactory.Received(1).CreateCar(carParams, 2); // Проверяем, что метод CreateCar был вызван с параметрами carParams и номером 2
    }

    [Fact]
    public void TakeCar_Call_ShouldReturnAddedCars() // Проверяем, что метод TakeCar возвращает добавленные автомобили
    {
        // Arrange
        var carService = new CarService(); // Создаем объект класса CarService
        
        var engine = Substitute.For<IEngine>(); // Создаем мок двигателя
        engine.IsCompatible(Arg.Any<Customer>()).Returns(true); // Устанавливаем, что метод IsCompatible будет возвращать true для любого покупателя

        var car1 = new Car(engine, 1); // Создаем первый автомобиль с номером 1
        var car2 = new Car(engine, 2); // Создаем второй автомобиль с номером 2
        
        var carFactory = Substitute.For<ICarFactory<int>>(); // Создаем мок фабрики автомобилей
        carFactory.CreateCar(Arg.Any<int>(), Arg.Any<int>()).Returns(car1, car2); // Устанавливаем, что метод CreateCar будет возвращать автомобили car1 и car2
        
        var carParams = 1; // Создаем параметры для автомобиля

        // Act
        carService.AddCar(carFactory, carParams); // Добавляем первый автомобиль в список
        carService.AddCar(carFactory, carParams); // Добавляем второй автомобиль в список
        
        var actualCar1 = carService.TakeCar(new Customer("Test", 0, 0)); // Получаем первый автомобиль из списка
        var actualCar2 = carService.TakeCar(new Customer("Test", 0, 0)); // Получаем второй автомобиль из списка

        // Assert
        Assert.Equal(car1, actualCar1); // Проверяем, что первый автомобиль, который вернул метод TakeCar, совпадает с первым автомобилем, который был добавлен в список
        Assert.Equal(car2, actualCar2); // Проверяем, что второй автомобиль, который вернул метод TakeCar, совпадает со вторым автомобилем, который был добавлен в список
    }
}