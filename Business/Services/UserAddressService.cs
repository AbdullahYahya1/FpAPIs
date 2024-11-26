using AutoMapper;
using Business.IServices;
using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.IRepositories;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UserAddressService : Service<UserAddress>, IUserAddressService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserAddressService(FPDbContext context, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<ResponseModel<bool>> AddAddress(PostAddressDto postAddressDto)
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
                //var type = _httpContextAccessor.HttpContext.User?.FindFirst("UserType")?.Value;
                var UserAdress = _mapper.Map<UserAddress>(postAddressDto);
                UserAdress.UserId = currentUserId;
                await _unitOfWork.UserAddresses.AddAsync(UserAdress);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel<bool> { IsSuccess = true, Result = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool> { IsSuccess = false, Message = "ErrorFound" };
            }
        }
        public async Task<ResponseModel<List<GetAddressDto>>> GetAddresses()
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
                var adresses = await _unitOfWork.UserAddresses.GetUserAddressesAsync(currentUserId);
                var adressesDto = _mapper.Map<List<GetAddressDto>>(adresses);
                return new ResponseModel<List<GetAddressDto>> { Result = adressesDto, IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetAddressDto>> { IsSuccess = false, Message = "ErrorFound" };
            }
        }

        public async Task<ResponseModel<bool>> UpdateAddress(UpdateAddressDto updateAddressDto)
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
                var address = await _unitOfWork.UserAddresses.GetByIdAsync(updateAddressDto.AddressId);
                if (address == null)
                {
                    return new ResponseModel<bool> { IsSuccess = false, Message = "AddressNotFound" };
                }
                if (address.UserId != currentUserId)
                {
                    return new ResponseModel<bool> { IsSuccess = false, Message = "Unauthorized" };
                }
                address = _mapper.Map(updateAddressDto, address);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel<bool> { IsSuccess = true, Result = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool> { IsSuccess = false, Message = "ErrorFound" };
            }
        }
    }
}
