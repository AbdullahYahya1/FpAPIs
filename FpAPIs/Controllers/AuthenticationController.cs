 using Microsoft.AspNetCore.Mvc;
using Business.IServices;
using DataAccess.DTOs;

//AB
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService, ILogger<AuthenticationController> logger)
    {
        _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] PostUserDto userDto)
    {
        var res = await _userService.CreateUser(userDto);
        return Ok(res);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateModel model)
    {
        var tokens = await _userService.Authenticate(model.EmailORUserName, model.Password);
        return Ok(tokens);
    }

    [HttpPost("Customer/Login")]
    public async Task<IActionResult> CustomerLogin([FromBody] CustomerAuthenticateDto model)
    {
        var tokens = await _userService.CustomerAuthenticate(model.Phone);
        return Ok(tokens);
    }


    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
    {
        var response = await _userService.RefreshToken(tokenRequest);
        return Ok(response);
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        var response = await _userService.ResetPassword(resetPasswordDto);
        return Ok(response);
    }

    [HttpPost("ForgetPassword")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto forgetPasswordDto)
    {
        var response = await _userService.ForgetPassword(forgetPasswordDto);
        return Ok(response);
    }
}
