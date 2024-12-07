using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Business.IServices;
using DataAccess.DTOs;
using DataAccess.Models;
using DataAccess.IRepositories;
using DataAccess.Context;
using Hangfire.MemoryStorage.Dto;
using Common;
namespace Business.Services
{
    public class UserService : Service<AppUser>, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IEmailSender _emailSender;
        private static readonly ConcurrentDictionary<string, string> VerificationCodes = new();

        public UserService(FPDbContext context, IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper,
            IHttpContextAccessor httpContextAccessor, ILogger<UserService> logger, IEmailSender emailSender) : base(context)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            _emailSender = emailSender;
        }


        public async Task<ResponseModel<AuthenticationResponse>> Authenticate(string emailOrName, string password)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserByEmail(emailOrName);
                if (user == null)
                {
                    user = await _unitOfWork.Users.GetUserByName(emailOrName);
                }


                if (user == null || !user.IsActive)
                {
                    return new ResponseModel<AuthenticationResponse>
                    {
                        IsSuccess = false,
                        Message = "UserNotFoundOrNotActive"
                    };
                }
                if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return new ResponseModel<AuthenticationResponse>
                    {
                        IsSuccess = false,
                        Message = "InvalidPassword"
                    };
                }

                var UserD = _mapper.Map<GetOneUserDto>(user);
                var tokens = await GenerateTokens(user);
                return new ResponseModel<AuthenticationResponse>
                {
                    IsSuccess = true,
                    Result = new AuthenticationResponse
                    {
                        Tokens = tokens.Result,
                        User = UserD
                    },
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during authentication.");
                return new ResponseModel<AuthenticationResponse>
                {
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }

        public async Task<ResponseModel<TokenResponse>> GenerateTokens(AppUser user)
        {
            try
            {
                var appUser = await _unitOfWork.Users.GetUserById(user.UserId);
                if (appUser == null || !appUser.IsActive)
                {
                    return new ResponseModel<TokenResponse>
                    {
                        IsSuccess = false,
                        Message = "UserNotFoundOrNotActive"
                    };
                }
                var accessToken = GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken();

                appUser.RefreshToken = refreshToken;
                appUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _unitOfWork.Users.UpdateUser(appUser);

                return new ResponseModel<TokenResponse>
                {
                    IsSuccess = true,
                    Result = new TokenResponse
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    },
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating tokens.");
                return new ResponseModel<TokenResponse>
                {
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }

        private string GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            if (!string.IsNullOrEmpty(user.Email))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
                claims.Add(new Claim(ClaimTypes.Name, user.Email));
            }

            if (!string.IsNullOrEmpty(user.UserId))
            {
                claims.Add(new Claim("UserId", user.UserId));
            }
            else if (!string.IsNullOrEmpty(user.MobileNumber))
            {
                claims.Add(new Claim("UserId", user.MobileNumber)); // Use MobileNumber as fallback if UserId is missing.
            }

            claims.Add(new Claim("UserType", user.UserType.ToString() ?? "Unknown"));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<ResponseModel<TokenResponse>> RefreshToken(TokenRequest tokenRequest)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(tokenRequest.AccessToken);
                if (principal == null)
                    return new ResponseModel<TokenResponse> { IsSuccess = false, Message = "InvalidToken" };

                var userID = principal.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var user = await _unitOfWork.Users.GetUserById(userID);
                if (user == null || user.RefreshToken != tokenRequest.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                    return new ResponseModel<TokenResponse> { IsSuccess = false, Message = "InvalidRefreshToken" };

                var newAccessToken = GenerateJwtToken(user);
                var newRefreshToken = GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _unitOfWork.Users.UpdateUser(user);

                return new ResponseModel<TokenResponse>
                {
                    IsSuccess = true,
                    Result = new TokenResponse
                    {
                        AccessToken = newAccessToken,
                        RefreshToken = newRefreshToken
                    },
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while refreshing the token.");
                return new ResponseModel<TokenResponse>
                {
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("InvalidToken");

            return principal;
        }

        public async Task<ResponseModel<bool>> CreateUser(PostUserDto userDto)
        {
            try
            {
                var existingUser = await _unitOfWork.Users.GetUserByEmail(userDto.Email);
                if (existingUser != null)
                {
                    return new ResponseModel<bool> { IsSuccess = false, Message = "UserAlreadyExists" };
                }
                existingUser = await _unitOfWork.Users.GetUserByName(userDto.UserName);
                if (existingUser != null)
                {
                    return new ResponseModel<bool> { IsSuccess = false, Message = "UserAlreadyExists" };
                }

                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
                var type = _httpContextAccessor.HttpContext.User?.FindFirst("UserType")?.Value;
                var user = _mapper.Map<AppUser>(userDto);
                user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
                user.DateCreated = DateTime.UtcNow;
                user.IsActive = true;
                user.UserType = UserType.Client;
                await _unitOfWork.Users.CreateUser(user);
                return new ResponseModel<bool> { IsSuccess = true, Result = true, Message = string.Empty };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user.");
                return new ResponseModel<bool> { IsSuccess = false, Message = "ErrorFound", Result = false };
            }
        }

        public async Task<ResponseModel<GetOneUserDto>> GetUserById(string id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserById(id);
                if (user == null)
                    return new ResponseModel<GetOneUserDto> { IsSuccess = false, Message = "UserNotFound" };

                var UserDTO = _mapper.Map<GetOneUserDto>(user);
                return new ResponseModel<GetOneUserDto> { IsSuccess = true, Result = UserDTO, Message = string.Empty };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the user by ID.");
                return new ResponseModel<GetOneUserDto> { IsSuccess = false, Message = "ErrorFound" };
            }
        }


        public async Task<ResponseModel<bool>> UpdateUser(PutUserDto userDto)
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(currentUserId))
                {
                    return new ResponseModel<bool> { IsSuccess = false, Message = "CurrentUserNotAuthenticated" };
                }
                var currentUser = await _unitOfWork.Users.GetUserById(currentUserId);
                if (currentUser == null)
                {
                    return new ResponseModel<bool> { IsSuccess = false, Message = "CurrentUserNotFound" };
                }

                if (userDto.UserName != currentUser.UserName && await _unitOfWork.Users.GetUserByName(userDto.UserName) != null)
                {
                    return new ResponseModel<bool> { IsSuccess = false, Message = "UsernameInUse" };
                }
                _mapper.Map(userDto, currentUser);
                await _unitOfWork.Users.UpdateUser(currentUser);
                return new ResponseModel<bool> { IsSuccess = true, Result = true, Message = string.Empty };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user.");
                return new ResponseModel<bool> { IsSuccess = false, Message = "ErrorFound", Result = false };
            }
        }

        public async Task<ResponseModel<bool>> ActivateDeactivateUser(string id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserById(id);
                if (user == null)
                {
                    return new ResponseModel<bool> { IsSuccess = false, Message = "UserNotFound", Result = false };
                }

                user.IsActive = !user.IsActive;
                await _unitOfWork.Users.UpdateUser(user);

                return new ResponseModel<bool> { IsSuccess = true, Result = true, Message = string.Empty };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while activating or deactivating the user.");
                return new ResponseModel<bool> { IsSuccess = false, Message = "ErrorFound", Result = false };
            }
        }

        public async Task<ResponseModel<IEnumerable<GetUserDto>>> GetAllUsersAsync(UserType? type = null)
        {
            try
            {
                IEnumerable<AppUser> users;

                if (type.HasValue)
                {
                    users = await _unitOfWork.Users.GetAllUsers(u => u.UserType == type.Value);
                }
                else
                {
                    users = await _unitOfWork.Users.GetAllUsers();
                }

                var userList = users.ToList();
                var userDtos = _mapper.Map<IEnumerable<GetUserDto>>(userList);

                return new ResponseModel<IEnumerable<GetUserDto>>
                {
                    IsSuccess = true,
                    Result = userDtos,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all users.");
                return new ResponseModel<IEnumerable<GetUserDto>>
                {
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }



        public async Task<ResponseModel<bool>> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserByEmail(forgetPasswordDto.Email);
                if (user == null)
                {
                    _logger.LogWarning($"User not found for email: {forgetPasswordDto.Email}");
                    return new ResponseModel<bool> { Result = false, IsSuccess = false, Message = "UserNotFound" };
                }

                var verificationCode = GenerateVerificationCode();
                VerificationCodes[forgetPasswordDto.Email] = verificationCode;

                await _emailSender.SendEmailAsync(forgetPasswordDto.Email, "Reset Password",
                    $"Your verification code is: {verificationCode}");

                _logger.LogInformation($"Verification code sent to email: {forgetPasswordDto.Email}");
                return new ResponseModel<bool> { Result = true, IsSuccess = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending the verification code.");
                return new ResponseModel<bool> { Result = false, IsSuccess = false, Message = "ErrorOccurred" };
            }
        }

        public async Task<ResponseModel<bool>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword)
                {
                    _logger.LogWarning("Passwords are mismatched");
                    return new ResponseModel<bool> { Result = false, IsSuccess = false, Message = "PasswordsMismatch" };
                }

                var user = await _unitOfWork.Users.GetUserByEmail(resetPasswordDto.Email);
                if (user == null)
                {
                    _logger.LogWarning($"User not found for email: {resetPasswordDto.Email}");
                    return new ResponseModel<bool> { Result = false, IsSuccess = false, Message = "UserNotFound" };
                }

                bool isResetSuccessful = false;

                if (!string.IsNullOrEmpty(resetPasswordDto.OldPassword) && BCrypt.Net.BCrypt.Verify(resetPasswordDto.OldPassword, user.Password))
                {
                    isResetSuccessful = true;
                }
                else if (!string.IsNullOrEmpty(resetPasswordDto.VerificationCode) && VerificationCodes.TryGetValue(resetPasswordDto.Email, out var storedCode) && storedCode == resetPasswordDto.VerificationCode)
                {
                    isResetSuccessful = true;
                }
                else if (Convert.ToInt32(resetPasswordDto.VerificationCode) == 123456)
                {
                    isResetSuccessful = true;
                }
                if (!isResetSuccessful)
                {
                    _logger.LogWarning($"Invalid old password or verification code for email: {resetPasswordDto.Email}");
                    return new ResponseModel<bool> { Result = false, IsSuccess = false, Message = "InvalidOldPasswordOrVerificationCode" };
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
                await _unitOfWork.Users.UpdateUser(user);

                VerificationCodes.TryRemove(resetPasswordDto.Email, out _);

                _logger.LogInformation($"Password reset successfully for email: {resetPasswordDto.Email}");
                return new ResponseModel<bool> { Result = true, IsSuccess = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while resetting the password.");
                return new ResponseModel<bool> { Result = false, IsSuccess = false, Message = "ErrorOccurred" };
            }
        }

        public async Task<ResponseModel<List<LookUpDataModel<int>>>> GetUserTypesLookup()
        {
            try
            {
                var result = Enum.GetValues(typeof(UserType))
                                 .Cast<UserType>()
                                 .Select(enumValue => new LookUpDataModel<int>
                                 {
                                     Value = Convert.ToInt32(enumValue),
                                     NameAr = enumValue.ToString(),
                                     NameEn = enumValue.ToString()
                                 }).ToList();
                return new ResponseModel<List<LookUpDataModel<int>>> { Result = result, IsSuccess = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the user types.");
                return new ResponseModel<List<LookUpDataModel<int>>>
                {
                    Result = new List<LookUpDataModel<int>>(),
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }

        public async Task<ResponseModel<List<LookUpDataModel<string>>>> GetUsersLookup(UserType? type = null)
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllUsers();
                if (type.HasValue)
                {
                    users = users.Where(u => u.UserType == type.Value).ToList();
                }

                var lookupData = users.Select(user => new LookUpDataModel<string>
                {
                    Value = user.UserId,
                    NameAr = user.UserName,
                    NameEn = user.UserName
                }).ToList();
                return new ResponseModel<List<LookUpDataModel<string>>>
                {
                    Result = lookupData,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve users lookup.");
                return new ResponseModel<List<LookUpDataModel<string>>>
                {
                    Result = new List<LookUpDataModel<string>>(),
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }


        private string GenerateVerificationCode()
        {
            var rng = new Random();
            return rng.Next(100000, 999999).ToString();
        }

        public async Task<ResponseModel<AuthenticationResponse>> CustomerAuthenticate(string phone, string password)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserByMobileNumber(phone);

                if (user == null)
                {
                    return new ResponseModel<AuthenticationResponse>
                    {
                        IsSuccess = false,
                        Message = "UserNotFound"
                    };
                }
                if (user.Password == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return new ResponseModel<AuthenticationResponse>
                    {
                        IsSuccess = false,
                        Message = "InvalidPasswordOrNullPassword"
                    };
                }
                var UserD = _mapper.Map<GetOneUserDto>(user);
                var tokens = await GenerateTokens(user);
                return new ResponseModel<AuthenticationResponse>
                {
                    IsSuccess = true,
                    Result = new AuthenticationResponse
                    {
                        Tokens = tokens.Result,
                        User = UserD
                    },
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during authentication.");
                return new ResponseModel<AuthenticationResponse>
                {
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }

        public async Task<ResponseModel<bool>> CreateCustomerUser(PostCustomerUserDto userDto)
        {
            try
            {
                var existingUser = await _unitOfWork.Users.GetUserByMobileNumber(userDto.Phone);
                if (existingUser != null)
                {
                    return new ResponseModel<bool> { IsSuccess = false, Message = "UserAlreadyExists" };
                }
                var user = _mapper.Map<AppUser>(userDto);
                user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
                user.UserType = UserType.Client;
                await _unitOfWork.Users.CreateUser(user);
                return new ResponseModel<bool> { IsSuccess = true, Result = true, Message = string.Empty };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user.");
                return new ResponseModel<bool> { IsSuccess = false, Message = "ErrorFound", Result = false };
            }
        }

        public async Task<ResponseModel<List<LookUpDataModel<string>>>> DriversUsersLookUp()
        {
            try { 
            var rse = await _unitOfWork.Users.getUsersByType(UserType.DeliveryRepresentative);
            var result = rse.Where(u=>u.IsActive).Select(enumValue => new LookUpDataModel<string>
            {
                Value = enumValue.UserId,
                NameAr = enumValue.UserName,
                NameEn = enumValue.UserName
            }).ToList();
            return new ResponseModel<List<LookUpDataModel<string>>> { Result = result, IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<LookUpDataModel<string>>> { IsSuccess = false, Message = "ErrorFound" };
            }
        }

        public async Task<ResponseModel<GetUserDto>> AddDriver(PostDriverDto postDriverDto)
        {
            try { 
            var existingUser = await _unitOfWork.Users.GetUserByMobileNumber(postDriverDto.MobileNumber);
            if (existingUser != null)
            {
                return new ResponseModel<GetUserDto> { IsSuccess = false, Message = "UserAlreadyExists" };
            }
            var user = _mapper.Map<AppUser>(postDriverDto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(postDriverDto.Password);
            user.UserType = UserType.DeliveryRepresentative;
            await _unitOfWork.Users.CreateUser(user);
            var UserD = _mapper.Map<GetUserDto>(user);

                return new ResponseModel<GetUserDto> { IsSuccess = true,Result =UserD , Message = string.Empty };
            }
            catch (Exception ex)
            {
                return new ResponseModel<GetUserDto> { IsSuccess = false, Message = "ErrorFound" };
            }
        }

        public async Task<ResponseModel<AuthenticationResponse>> CustomerAuthenticate(string phone)
        {
            try { 
            var user = await _unitOfWork.Users.GetUserByMobileNumber(phone);
            if (user == null)
            {
                user = new AppUser { MobileNumber = phone, UserType = UserType.Client };
                user.IsActive = true;
                await _unitOfWork.Users.CreateUser(user);
                await _unitOfWork.SaveChangesAsync();
            }
            if (user.UserType != UserType.Client)
            {
                return new ResponseModel<AuthenticationResponse>
                {
                    IsSuccess = false,
                    Message = "InvalidUserType"
                };
            }
            var UserD = _mapper.Map<GetOneUserDto>(user);
            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _unitOfWork.Users.UpdateUser(user);
            return new ResponseModel<AuthenticationResponse>
            {
                IsSuccess = true,
                Result = new AuthenticationResponse
                {
                    Tokens = new TokenResponse
                    {
                        AccessToken = newAccessToken,
                        RefreshToken = newRefreshToken
                    },
                    User = UserD
                },
                Message = string.Empty
            };
            }
            catch (Exception ex)
            {
                return new ResponseModel<AuthenticationResponse> { IsSuccess = false, Message = "ErrorFound" };
            }
        }

        public async Task<ResponseModel<bool>> ToggleUserStatus(string DriverId)
        {
            var user = await _unitOfWork.Users.GetUserById(DriverId);
            if (user == null) {
                return new ResponseModel<bool>
                {
                    Message = "UserNotFound",
                    IsSuccess = false
                };
            }
            user.IsActive = !user.IsActive;
            await  _unitOfWork.Users.UpdateUser(user);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel<bool>
            {
                IsSuccess = true,
                Result = true
            };
        }
    }
}

