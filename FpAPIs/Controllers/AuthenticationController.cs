using Microsoft.AspNetCore.Mvc;
using Business.IServices;
using DataAccess.DTOs;
using DataAccess.Models;
using Common;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IUserService userService, ILogger<AuthenticationController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<ResponseModel<bool>>> Register([FromBody] PostUserDto userDto)
    {
        _logger.LogInformation($"Input received for Register: {System.Text.Json.JsonSerializer.Serialize(userDto)}");

        var res = await _userService.CreateUser(userDto);

        _logger.LogInformation($"Output for Register: {System.Text.Json.JsonSerializer.Serialize(res)}");
        return Ok(res);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<ResponseModel<AuthenticationResponse>>> Login([FromBody] AuthenticateModel model)
    {
        _logger.LogInformation($"Input received for Login: {System.Text.Json.JsonSerializer.Serialize(model)}");

        var tokens = await _userService.Authenticate(model.EmailORUserName, model.Password);

        _logger.LogInformation($"Output for Login: {System.Text.Json.JsonSerializer.Serialize(tokens)}");
        return Ok(tokens);
    }

    [HttpPost("Customer/Login")]
    public async Task<ActionResult<ResponseModel<AuthenticationResponse>>> CustomerLogin([FromBody] CustomerAuthenticateDto model)
    {
        _logger.LogInformation($"Input received for CustomerLogin: {System.Text.Json.JsonSerializer.Serialize(model)}");

        var tokens = await _userService.CustomerAuthenticate(model.Phone);

        _logger.LogInformation($"Output for CustomerLogin: {System.Text.Json.JsonSerializer.Serialize(tokens)}");
        return Ok(tokens);
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult<ResponseModel<TokenResponse>>> RefreshToken([FromBody] TokenRequest tokenRequest)
    {
        _logger.LogInformation($"Input received for RefreshToken: {System.Text.Json.JsonSerializer.Serialize(tokenRequest)}");

        var response = await _userService.RefreshToken(tokenRequest);

        _logger.LogInformation($"Output for RefreshToken: {System.Text.Json.JsonSerializer.Serialize(response)}");
        return Ok(response);
    }

    //[HttpPost("ResetPassword")]
    //public async Task<ActionResult<ResponseModel<bool>>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    //{
    //    _logger.LogInformation($"Input received for ResetPassword: {System.Text.Json.JsonSerializer.Serialize(resetPasswordDto)}");
    //
    //    var response = await _userService.ResetPassword(resetPasswordDto);
    //
    //    _logger.LogInformation($"Output for ResetPassword: {System.Text.Json.JsonSerializer.Serialize(response)}");
    //    return Ok(response);
    //}

    //[HttpPost("ForgetPassword")]
    //public async Task<ActionResult<ResponseModel<bool>>> ForgetPassword([FromBody] ForgetPasswordDto forgetPasswordDto)
    //{
    //    _logger.LogInformation($"Input received for ForgetPassword: {System.Text.Json.JsonSerializer.Serialize(forgetPasswordDto)}");
    //
    //    var response = await _userService.ForgetPassword(forgetPasswordDto);
    //
    //    _logger.LogInformation($"Output for ForgetPassword: {System.Text.Json.JsonSerializer.Serialize(response)}");
    //    return Ok(response);
    //}
}
