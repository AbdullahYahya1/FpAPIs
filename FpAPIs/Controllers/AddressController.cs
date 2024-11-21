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
    public class AddressController : ControllerBase
    {
        private readonly IUserAddressService _userAddressService;

        public AddressController(IUserAddressService userAddressService)
        {
            _userAddressService = userAddressService;
        }

        [HttpPost("CreateAddress")]
        public async Task<ActionResult<ResponseModel<bool>>> CreateAddress(PostAddressDto postAddressDto)
        {
            var res = await _userAddressService.AddAddress(postAddressDto);
            return Ok(res);
        }

        [HttpPost("GetAddresses")]
        public async Task<ActionResult<ResponseModel<List<GetAddressDto>>>> GetAddresses()
        {
            var res = await _userAddressService.GetAddresses();
            return Ok(res);
        }
    }
}
