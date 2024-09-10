using Business.IServices;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FpAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpGet("GetCartItems")]
        public async Task<IActionResult> GetCartItems()
        {
            var res = await _cartItemService.GetAllCartItemAsync();
            return Ok(res); 
        }

        [HttpPost("AddCartItem")]
        public async Task<IActionResult> AddCartItem([FromBody] CartItemDto cartItemDto)
        {
            var res = await _cartItemService.AddCartItemAsync(cartItemDto);
            return Ok(res);
        }
        [HttpPost("RemoveCartItem")]
        public async Task<IActionResult> RemoveCartItem([FromBody] CartItemDto cartItemDto)
        {
            var res = await _cartItemService.RemoveCartItemAsync(cartItemDto);
            return Ok(res);
        }
    }
}
