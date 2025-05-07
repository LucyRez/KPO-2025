using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversalCarShop.UseCases.Customers;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("[action]")]
        public IActionResult AddCustomer(
            [FromQuery] string name,
            [FromQuery] int legPower,
            [FromQuery] int handPower
        )
        {
            _customerService.AddCustomerPending(name, legPower, handPower);
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult GetAllCustomers()
        {
            return Ok(_customerService.GetAllCustomers());
        }
    }
}
