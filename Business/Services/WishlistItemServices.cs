using AutoMapper;
using Business.IServices;
using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.IRepositories;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class WishlistItemService : Service<WishlistItem>, IWishlistItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WishlistItemService(FPDbContext context, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel<bool>> AddWishlistItemAsync(WishlistItemDto item)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(currentUserId) || item?.ProductId == null)
            {
                return new ResponseModel<bool> { IsSuccess = false, Message = "Invalid user or product" };
            }

            var wishlistItem = _mapper.Map<WishlistItem>(item);
            wishlistItem.UserId = currentUserId;
            await _unitOfWork.WishlistItems.AddAsync(wishlistItem);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseModel<bool> { IsSuccess = true, Result = true };
        }

        public async Task<ResponseModel<bool>> RemoveWishlistItemAsync(WishlistItemDto item)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(currentUserId) || item?.ProductId == null)
            {
                return new ResponseModel<bool> { IsSuccess = false, Message = "Invalid user or product" };
            }

            var wishlistItem = await _unitOfWork.WishlistItems
                .GetAll()
                .FirstOrDefaultAsync(wi => wi.UserId == currentUserId && wi.ProductId == item.ProductId);

            if (wishlistItem == null)
            {
                return new ResponseModel<bool> { IsSuccess = false, Message = "Wishlist item not found" };
            }

            await _unitOfWork.WishlistItems.DeleteAsync(wishlistItem.WishlistItemId);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseModel<bool> { IsSuccess = true, Result = true };
        }

        public async Task<ResponseModel<List<GetProductDto>>> GetAllWishlistItemsAsync()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ResponseModel<List<GetProductDto>> { IsSuccess = false, Message = "Invalid user" };
            }

            var wishlistItems = await _unitOfWork.WishlistItems
                .GetAll()
                .Where(wi => wi.UserId == currentUserId)
                .Include(wi => wi.Product)
                .ToListAsync();

            if (!wishlistItems.Any())
            {
                return new ResponseModel<List<GetProductDto>> { IsSuccess = false, Message = "No wishlist items found" };
            }

            var productDtos = wishlistItems.Select(wi => _mapper.Map<GetProductDto>(wi.Product)).ToList();

            return new ResponseModel<List<GetProductDto>> { IsSuccess = true, Result = productDtos };
        }
    }
}
