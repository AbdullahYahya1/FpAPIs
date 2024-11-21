using Business.IServices;
using DataAccess.DTOs;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task<ActionResult<ResponseModel<List<GetProductDto>>>> GetCartItems()
        {
            var res = await _cartItemService.GetAllCartItemAsync();
            return Ok(res);
        }

        [HttpPost("AddCartItem")]
        public async Task<ActionResult<ResponseModel<bool>>> AddCartItem([FromBody] CartItemDto cartItemDto)
        {
            var res = await _cartItemService.AddCartItemAsync(cartItemDto);
            return Ok(res);
        }

        [HttpPost("RemoveCartItem")]
        public async Task<ActionResult<ResponseModel<bool>>> RemoveCartItem([FromBody] CartItemDto cartItemDto)
        {
            var res = await _cartItemService.RemoveCartItemAsync(cartItemDto);
            return Ok(res);
        }
    }
}
