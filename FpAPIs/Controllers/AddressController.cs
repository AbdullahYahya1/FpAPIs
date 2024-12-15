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
    public class AddressController : ControllerBase
    {
        private readonly IUserAddressService _userAddressService;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IUserAddressService userAddressService, ILogger<AddressController> logger)
        {
            _userAddressService = userAddressService;
            _logger = logger;
        }

        [CustomAuthorize([UserType.Client])]
        [HttpPost("CreateAddress")]
        public async Task<ActionResult<ResponseModel<bool>>> CreateAddress(PostAddressDto postAddressDto)
        {
            _logger.LogInformation($"Input received for CreateAddress: {System.Text.Json.JsonSerializer.Serialize(postAddressDto)}");

            var res = await _userAddressService.AddAddress(postAddressDto);

            _logger.LogInformation($"Output for CreateAddress: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [CustomAuthorize([UserType.Client])]
        [HttpGet("GetAddresses")]
        public async Task<ActionResult<ResponseModel<List<GetAddressDto>>>> GetAddresses()
        {
            _logger.LogInformation("Request received for GetAddresses.");

            var res = await _userAddressService.GetAddresses();

            _logger.LogInformation($"Output for GetAddresses: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [CustomAuthorize([UserType.Client])]
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateAddress(UpdateAddressDto updateAddressDto)
        {
            _logger.LogInformation($"Input received for UpdateAddress: {System.Text.Json.JsonSerializer.Serialize(updateAddressDto)}");

            var res = await _userAddressService.UpdateAddress(updateAddressDto);

            _logger.LogInformation($"Output for UpdateAddress: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }
    }
}
