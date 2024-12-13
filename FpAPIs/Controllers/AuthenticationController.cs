using Microsoft.AspNetCore.Mvc;
using Business.IServices;
using DataAccess.DTOs;
using DataAccess.Models;
using Common;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<ResponseModel<bool>>> Register([FromBody] PostUserDto userDto)
    {
        var res = await _userService.CreateUser(userDto);
        return Ok(res);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<ResponseModel<AuthenticationResponse>>> Login([FromBody] AuthenticateModel model)
    {
        var tokens = await _userService.Authenticate(model.EmailORUserName, model.Password);
        return Ok(tokens);
    }

    [HttpPost("Customer/Login")]
    public async Task<ActionResult<ResponseModel<AuthenticationResponse>>> CustomerLogin([FromBody] CustomerAuthenticateDto model)
    {
        var tokens = await _userService.CustomerAuthenticate(model.Phone);
        return Ok(tokens);
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult<ResponseModel<TokenResponse>>> RefreshToken([FromBody] TokenRequest tokenRequest)
    {
        var response = await _userService.RefreshToken(tokenRequest);
        return Ok(response);
    }

    //[HttpPost("ResetPassword")]
    //public async Task<ActionResult<ResponseModel<bool>>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    //{
    //    var response = await _userService.ResetPassword(resetPasswordDto);
    //    return Ok(response);
    //}

    //[HttpPost("ForgetPassword")]
    //public async Task<ActionResult<ResponseModel<bool>>> ForgetPassword([FromBody] ForgetPasswordDto forgetPasswordDto)
    //{
    //    var response = await _userService.ForgetPassword(forgetPasswordDto);
    //    return Ok(response);
    //}
}
