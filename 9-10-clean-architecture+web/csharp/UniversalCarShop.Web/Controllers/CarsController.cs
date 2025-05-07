using Microsoft.AspNetCore.Mvc;
using UniversalCarShop.UseCases.Cars;

namespace UniversalCarShop.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly ICarInventoryService _carInventoryService;

    public CarsController(ICarInventoryService carInventoryService)
    {
        _carInventoryService = carInventoryService;
    }
    
    [HttpPost("[action]")]
    public IActionResult AddPedalCar([FromQuery] int pedalSize)
    {
        _carInventoryService.AddPedalCarPending(pedalSize);
        return Ok();
    }

    [HttpPost("[action]")]
    public IActionResult AddHandCar()
    {
        _carInventoryService.AddHandCarPending();
        return Ok();
    }
}
