using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FpAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateAddress()
        {
            return Ok(); 
        }
    }
}
