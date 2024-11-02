using AutoMapper;
using Business.IServices;
using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.IRepositories;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CartItemService : Service<CartItem>, ICartItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartItemService(FPDbContext context, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel<bool>> AddCartItemAsync(CartItemDto item)
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(currentUserId) || item?.ProductId == null)
                {
                    return new ResponseModel<bool> { IsSuccess = false, Message = "Invalid user or product" };
                }
                var cartItem = _mapper.Map<CartItem>(item);
                cartItem.UserId = currentUserId;
                await _unitOfWork.CartItems.AddAsync(cartItem);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel<bool>() { IsSuccess = true, Result = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>()
                {
                    IsSuccess = false,
                    Message = "ErrorFound"

                };
            }
        }
        public async Task<ResponseModel<bool>> RemoveCartItemAsync(CartItemDto item)
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(currentUserId) || item?.ProductId == null)
                {
                    return new ResponseModel<bool> { IsSuccess = false, Message = "Invalid user or product" };
                }
                var cartItem = await _unitOfWork.CartItems
                    .GetAll()
                    .FirstOrDefaultAsync(ci => ci.UserId == currentUserId && ci.ProductId == item.ProductId);
                if (cartItem == null)
                {
                    return new ResponseModel<bool> { IsSuccess = false, Message = "Cart item not found" };
                }
                await _unitOfWork.CartItems.DeleteAsync(cartItem.CartItemId);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel<bool> { IsSuccess = true, Result = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>()
                {
                    IsSuccess = false,
                    Message = "ErrorFound"

                };
            }
        }

        public async Task<ResponseModel<List<GetProductDto>>> GetAllCartItemAsync()
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return new ResponseModel<List<GetProductDto>> { IsSuccess = false, Message = "Invalid user" };
                }
                var cartItems = await _unitOfWork.CartItems.GetCartItemProductsByUserID(currentUserId);
                if (cartItems == null || !cartItems.Any())
                {
                    return new ResponseModel<List<GetProductDto>> { IsSuccess = false, Message = "Cart items not found" };
                }
                var productDtos = cartItems.Select(item => _mapper.Map<GetProductDto>(item.Product)).ToList();
                return new ResponseModel<List<GetProductDto>> { IsSuccess = true, Result = productDtos };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetProductDto>>()
                {
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }
    }
}