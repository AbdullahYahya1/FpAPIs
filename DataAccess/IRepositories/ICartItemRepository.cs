﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface ICartItemRepository:IRepository<CartItem>
    {
        Task<List<CartItem>> GetCartItemProductsByUserID(string userId);
        Task RemoveCartItemsByProductIds(List<int> productIds);
        Task RemoveCartItemsForUserByProductIds(string userId, List<int> productIds);
    }
}
