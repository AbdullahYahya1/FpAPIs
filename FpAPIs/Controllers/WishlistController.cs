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
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistItemService _wishlistItemService;

        public WishlistController(IWishlistItemService wishlistItemService)
        {
            _wishlistItemService = wishlistItemService;
        }

        [HttpGet("GetWishlistItems")]
        public async Task<IActionResult> GetWishlistItems()
        {
            var res = await _wishlistItemService.GetAllWishlistItemsAsync();
            return Ok(res);
        }

        [HttpPost("AddWishlistItem")]
        public async Task<IActionResult> AddWishlistItem([FromBody] WishlistItemDto wishlistItemDto)
        {
            var res = await _wishlistItemService.AddWishlistItemAsync(wishlistItemDto);
            return Ok(res);
        }

        [HttpPost("RemoveWishlistItem")]
        public async Task<IActionResult> RemoveWishlistItem([FromBody] WishlistItemDto wishlistItemDto)
        {
            var res = await _wishlistItemService.RemoveWishlistItemAsync(wishlistItemDto);
            return Ok(res);
        }
    }
}
