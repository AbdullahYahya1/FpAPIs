using Business.IServices;
using Common;
using DataAccess.DTOs;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FpAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("DriversUsersLookUp")]
        [CustomAuthorize([UserType.Manager])]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<string>>>>> DriversUsersLookUp()
        {
            var res = await _userService.DriversUsersLookUp();
            return Ok(res);
        }
        [HttpPost("AddDriver")]
        [CustomAuthorize([UserType.Manager])]
        public async Task<ActionResult<ResponseModel<GetUserDto>>> AddDriver(PostDriverDto driverDto)
        {
            var res = await _userService.AddDriver(driverDto);
            return Ok(res);
        }
        [HttpPost("ToggleUserStatus/{UserId}")]
        [CustomAuthorize([UserType.Manager])]
        public async Task<ActionResult<ResponseModel<bool>>> ToggleUserStatus(string UserId)
        {
            var res = await _userService.ToggleUserStatus(UserId);
            return res;
        }
        [HttpPost("UpdateUser")]
        [CustomAuthorize([UserType.Client ,UserType.DeliveryRepresentative])]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateUser(PutUserDto userDto)
        {
            var res = await _userService.UpdateUser(userDto);
            return Ok(res);
        }
        [HttpGet("GetUsers")]
        [CustomAuthorize([UserType.Manager])]
        public async Task<ActionResult<ResponseModel<IEnumerable<GetUserDto>>>> GetUsers([FromQuery] UserType? type = null)
        {
            var res = await _userService.GetAllUsersAsync(type);
            return Ok(res);
        }
    }
}
