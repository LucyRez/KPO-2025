using System;
using MusicAPI.Models;

namespace MusicAPI.Repositories;

// Repository Interfaces
public interface IUserRepository { IEnumerable<User> GetUsers(); }
public interface IVinylRepository { IEnumerable<Vinyl> GetVinyls(); void AddVinyl(Vinyl vinyl); void UpdateStock(int id, int newStock); }
public interface IOrderRepository { IEnumerable<Order> GetOrders(); void AddOrder(Order order); void UpdateOrder(Order order); void UpdateOrderStatus(int id, string status); }

// Repository Implementations
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) => _context = context;
    public IEnumerable<User> GetUsers() => _context.Users.ToList();
}

public class VinylRepository : IVinylRepository
{
    private readonly AppDbContext _context;
    public VinylRepository(AppDbContext context) => _context = context;
    public IEnumerable<Vinyl> GetVinyls() => _context.Vinyl.ToList();
    public void AddVinyl(Vinyl vinyl) { _context.Vinyl.Add(vinyl); _context.SaveChanges(); }
    public void UpdateStock(int id, int newStock) { var vinyl = _context.Vinyl.Find(id); if (vinyl != null) { vinyl.Stock = newStock; _context.SaveChanges(); } }
}

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context) => _context = context;
    public IEnumerable<Order> GetOrders() => _context.Orders.ToList();
    public void AddOrder(Order order) { _context.Orders.Add(order); _context.SaveChanges(); }
    public void UpdateOrder(Order order) { _context.Orders.Update(order); _context.SaveChanges(); }
    public void UpdateOrderStatus(int id, string status) { var order = _context.Orders.Find(id); if (order != null) { order.Status = status; _context.SaveChanges(); } }
}
