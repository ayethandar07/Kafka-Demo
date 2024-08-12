using Microsoft.AspNetCore.Mvc;
using Producer.Models;
using Producer.Services;
using System.Text.Json;
using System.Threading.Tasks;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ProducerService _producerService;

        public InventoryController(ProducerService producerService)
        {
            _producerService = producerService;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInventory([FromBody] InventoryUpdateRequest request)
        {
            var message = JsonSerializer.Serialize(request);

            await _producerService.ProduceAsync("InventoryUpdate", message);

            return Ok("Inventory Updated Successfully...");
        }
    }
}
