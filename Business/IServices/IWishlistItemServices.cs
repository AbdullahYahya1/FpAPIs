using DataAccess.DTOs;
using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IWishlistItemService : IService<WishlistItem>
    {
        Task<ResponseModel<bool>> AddWishlistItemAsync(WishlistItemDto item);
        Task<ResponseModel<bool>> RemoveWishlistItemAsync(WishlistItemDto item);
        Task<ResponseModel<List<GetProductDto>>> GetAllWishlistItemsAsync();
    }
}
