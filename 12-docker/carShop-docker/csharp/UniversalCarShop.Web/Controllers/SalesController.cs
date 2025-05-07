using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversalCarShop.UseCases.Sales;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;

        public SalesController(ISalesService salesService)
        {
            _salesService = salesService;
        }

        [HttpPost("[action]")]
        public IActionResult SellCars()
        {
            _salesService.SellCars();
            return Ok();
        }
    }
}
