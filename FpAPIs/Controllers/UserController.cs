using Business.IServices;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FpAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("DriversUsersLookUp")]
        public async Task<IActionResult> DriversUsersLookUp()
        {
            var res = await _userService.DriversUsersLookUp();
            return Ok(res);
        }
        [HttpPost("AddDriver")]
        public async Task<IActionResult> AddDriver(PostDriverDto driverDto)
        {
            var res = await _userService.AddDriver(driverDto);
            return Ok(res);
        }
    }
}
