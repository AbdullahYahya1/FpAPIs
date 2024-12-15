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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("DriversUsersLookUp")]
        [CustomAuthorize([UserType.Manager])]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<string>>>>> DriversUsersLookUp()
        {
            _logger.LogInformation("Request received for DriversUsersLookUp.");

            var res = await _userService.DriversUsersLookUp();

            _logger.LogInformation($"Output for DriversUsersLookUp: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpPost("AddDriver")]
        [CustomAuthorize([UserType.Manager])]
        public async Task<ActionResult<ResponseModel<GetUserDto>>> AddDriver(PostDriverDto driverDto)
        {
            _logger.LogInformation($"Input received for AddDriver: {System.Text.Json.JsonSerializer.Serialize(driverDto)}");

            var res = await _userService.AddDriver(driverDto);

            _logger.LogInformation($"Output for AddDriver: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpPost("ToggleUserStatus/{UserId}")]
        [CustomAuthorize([UserType.Manager])]
        public async Task<ActionResult<ResponseModel<bool>>> ToggleUserStatus(string UserId)
        {
            _logger.LogInformation($"Input received for ToggleUserStatus: UserId = {UserId}");

            var res = await _userService.ToggleUserStatus(UserId);

            _logger.LogInformation($"Output for ToggleUserStatus: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpPost("UpdateUser")]
        [CustomAuthorize([UserType.Client, UserType.DeliveryRepresentative])]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateUser(PutUserDto userDto)
        {
            _logger.LogInformation($"Input received for UpdateUser: {System.Text.Json.JsonSerializer.Serialize(userDto)}");

            var res = await _userService.UpdateUser(userDto);

            _logger.LogInformation($"Output for UpdateUser: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpGet("GetUsers")]
        [CustomAuthorize([UserType.Manager])]
        public async Task<ActionResult<ResponseModel<IEnumerable<GetUserDto>>>> GetUsers([FromQuery] UserType? type = null)
        {
            _logger.LogInformation($"Request received for GetUsers with UserType filter: {type}");

            var res = await _userService.GetAllUsersAsync(type);

            _logger.LogInformation($"Output for GetUsers: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }
    }
}
