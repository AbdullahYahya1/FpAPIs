using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FpAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders() {
            return Ok();
        }
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder()
        {
            return Ok();
        }
    }
}
