using Business.IServices;
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
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<string>>>>> DriversUsersLookUp()
        {
            var res = await _userService.DriversUsersLookUp();
            return Ok(res);
        }
        [HttpPost("AddDriver")]
        public async Task<ActionResult<ResponseModel<bool>>> AddDriver(PostDriverDto driverDto)
        {
            var res = await _userService.AddDriver(driverDto);
            return Ok(res);
        }
        [HttpPost("UpdateUser")]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateUser(PutUserDto userDto)
        {
            var res = await _userService.UpdateUser(userDto);
            return Ok(res);
        }
    }
}
