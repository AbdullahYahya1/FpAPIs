using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FpAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public ProductController() { }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok();
        }
    }
}
