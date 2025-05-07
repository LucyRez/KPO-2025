using System;
using MusicAPI.Models;
using MusicAPI.Repositories;

namespace MusicAPI.Services;

public interface IOrderService { IEnumerable<Order> GetOrders(); void AddOrder(Order order); void UpdateOrder(Order order); void UpdateOrderStatus(int id, string status); }
public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    public OrderService(IOrderRepository repository) => _repository = repository;
    public IEnumerable<Order> GetOrders() => _repository.GetOrders();
    public void AddOrder(Order order) => _repository.AddOrder(order);
    public void UpdateOrder(Order order) => _repository.UpdateOrder(order);
    public void UpdateOrderStatus(int id, string status) => _repository.UpdateOrderStatus(id, status);
}