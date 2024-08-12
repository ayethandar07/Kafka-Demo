using Consumer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Consumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult ProcessInventoryUpdate([FromBody] InventoryUpdateRequest request)
        {
            _logger.LogInformation("Inventory update processed successfully.");
            return Ok("Inventory update processed successfully.");
        }
    }
}
