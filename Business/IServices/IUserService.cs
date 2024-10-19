using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.IServices;
using DataAccess.Models;
using DataAccess.DTOs;

namespace Business.IServices
{
    public interface IUserService : IService<AppUser>
    {
        Task<ResponseModel<bool>> CreateUser(PostUserDto userDto);
        Task<ResponseModel<bool>> CreateCustomerUser(PostCustomerUserDto userDto);
        Task<ResponseModel<bool>> AddDriver(PostDriverDto postDriverDto);

        Task<ResponseModel<AuthenticationResponse>> Authenticate(string emailOrName, string password);
        Task<ResponseModel<AuthenticationResponse>> CustomerAuthenticate(string Phone, string password);
        Task<ResponseModel<TokenResponse>> GenerateTokens(AppUser user);
        Task<ResponseModel<TokenResponse>> RefreshToken(TokenRequest tokenRequest);
        Task<ResponseModel<GetOneUserDto>> GetUserById(string id);
        Task<ResponseModel<bool>> UpdateUser(PutUserDto userDto);
        Task<ResponseModel<bool>> ActivateDeactivateUser(string id);
        Task<ResponseModel<IEnumerable<GetUserDto>>> GetAllUsersAsync(UserType? type = null);
        Task<ResponseModel<bool>> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<ResponseModel<bool>> ForgetPassword(ForgetPasswordDto forgetPasswordDto);
        Task<ResponseModel<List<LookUpDataModel<string>>>> DriversUsersLookUp();

    }
}
