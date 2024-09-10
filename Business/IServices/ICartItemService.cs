using DataAccess.DTOs;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface ICartItemService:IService<CartItem>
    {
        Task<ResponseModel<bool>> AddCartItemAsync(CartItemDto item);
        Task<ResponseModel<bool>> RemoveCartItemAsync(CartItemDto item);
        Task<ResponseModel<List<GetProductDto>>> GetAllCartItemAsync();
    }
}
