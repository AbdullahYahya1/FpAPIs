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
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistItemService _wishlistItemService;

        //public WishlistController(IWishlistItemService wishlistItemService)
        //{
        //    _wishlistItemService = wishlistItemService;
        //}

        //[HttpGet("GetWishlistItems")]
        //public async Task<ActionResult<ResponseModel<List<GetProductDto>>>> GetWishlistItems()
        //{
        //    var res = await _wishlistItemService.GetAllWishlistItemsAsync();
        //    return Ok(res);
        //}

        //[HttpPost("AddWishlistItem")]
        //public async Task<ActionResult<ResponseModel<bool>>> AddWishlistItem([FromBody] WishlistItemDto wishlistItemDto)
        //{
        //    var res = await _wishlistItemService.AddWishlistItemAsync(wishlistItemDto);
        //    return Ok(res);
        //}

        //[HttpPost("RemoveWishlistItem")]
        //public async Task<ActionResult<ResponseModel<bool>>> RemoveWishlistItem([FromBody] WishlistItemDto wishlistItemDto)
        //{
        //    var res = await _wishlistItemService.RemoveWishlistItemAsync(wishlistItemDto);
        //    return Ok(res);
        //}
    }
}
