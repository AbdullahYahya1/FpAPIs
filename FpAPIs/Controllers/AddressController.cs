using Business.IServices;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FpAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IUserAddressService _userAddressService;

        public AddressController(IUserAddressService userAddressService) {
            _userAddressService = userAddressService;
        }
        [HttpPost("CreateAddress")]
        public async Task<IActionResult> CreateAddress(PostAddressDto postAddressDto)
        {
            var res = await _userAddressService.AddAddress(postAddressDto);
            return Ok(res); 
        }

        [HttpPost("GetAddresses")]
        public async Task<IActionResult> GetAddresses()
        {
            var res = await _userAddressService.GetAddresses();
            return Ok(res);
        }
    }
}
