using System;
using Microsoft.AspNetCore.Mvc;
using MusicAPI.Models;
using MusicAPI.Repositories;
using MusicAPI.Services;

namespace MusicAPI.Controllers;

// Presentation Layer - Controllers
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repository;
    public UsersController(IUserRepository repository) => _repository = repository;
    [HttpGet] public IActionResult GetUsers() => Ok(_repository.GetUsers());
}

[Route("api/[controller]")]
[ApiController]
public class VinylsController : ControllerBase
{
    private readonly IVinylRepository _repository;
    public VinylsController(IVinylRepository repository) => _repository = repository;
    [HttpGet] public IActionResult GetVinyls() => Ok(_repository.GetVinyls());
    [HttpPost] public IActionResult AddVinyl(Vinyl vinyl) { _repository.AddVinyl(vinyl); return CreatedAtAction(nameof(GetVinyls), new { id = vinyl.RecordId }, vinyl); }
    [HttpPut("{id}/stock")] public IActionResult UpdateStock(int id, [FromBody] int newStock) { _repository.UpdateStock(id, newStock); return NoContent(); }
}

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;
    public OrdersController(IOrderService service) => _service = service;
    [HttpGet] public IActionResult GetOrders() => Ok(_service.GetOrders());
    [HttpPost] public IActionResult AddOrder(Order order) { _service.AddOrder(order); return CreatedAtAction(nameof(GetOrders), new { id = order.OrderId }, order); }
    [HttpPut("{id}")] public IActionResult UpdateOrder(int id, Order updatedOrder) { _service.UpdateOrder(updatedOrder); return NoContent(); }
    [HttpPatch("{id}/status")] public IActionResult UpdateOrderStatus(int id, [FromBody] string status) { _service.UpdateOrderStatus(id, status); return NoContent(); }
}
