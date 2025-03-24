using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversalCarShop.UseCases.PendingCommands;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class PendingChangesController : ControllerBase
    {
        private readonly IPendingCommandService _pendingCommandService;

        public PendingChangesController(IPendingCommandService pendingCommandService)
        {
            _pendingCommandService = pendingCommandService;
        }

        [HttpPost("[action]")]
        public IActionResult ApplyPendingChanges()
        {
            _pendingCommandService.SaveChanges();
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult UndoLastCommand()
        {
            _pendingCommandService.UndoLastCommand();
            return Ok();
        }
    }
}
