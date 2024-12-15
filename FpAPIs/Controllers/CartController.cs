using Business.IServices;
using Common;
using DataAccess.DTOs;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CartController> _logger;

        public CartController(ICartItemService cartItemService, ILogger<CartController> logger)
        {
            _cartItemService = cartItemService;
            _logger = logger;
        }

        [HttpGet("GetCartItems")]
        [CustomAuthorize([UserType.Client])]
        public async Task<ActionResult<ResponseModel<List<GetProductDto>>>> GetCartItems()
        {
            _logger.LogInformation("Request received for GetCartItems.");

            var res = await _cartItemService.GetAllCartItemAsync();

            _logger.LogInformation($"Output for GetCartItems: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpPost("AddCartItem")]
        [CustomAuthorize([UserType.Client])]
        public async Task<ActionResult<ResponseModel<bool>>> AddCartItem([FromBody] CartItemDto cartItemDto)
        {
            _logger.LogInformation($"Input received for AddCartItem: {System.Text.Json.JsonSerializer.Serialize(cartItemDto)}");

            var res = await _cartItemService.AddCartItemAsync(cartItemDto);

            _logger.LogInformation($"Output for AddCartItem: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpPost("RemoveCartItem")]
        [CustomAuthorize([UserType.Client])]
        public async Task<ActionResult<ResponseModel<bool>>> RemoveCartItem([FromBody] CartItemDto cartItemDto)
        {
            _logger.LogInformation($"Input received for RemoveCartItem: {System.Text.Json.JsonSerializer.Serialize(cartItemDto)}");

            var res = await _cartItemService.RemoveCartItemAsync(cartItemDto);

            _logger.LogInformation($"Output for RemoveCartItem: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }
    }
}
