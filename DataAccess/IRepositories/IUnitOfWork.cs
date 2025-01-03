﻿using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IProductRepository Products { get; }
        IBrandRepository Brands  { get; }
        IMaterialRepository Materials { get; }
        ICategoryRepository Categorys { get; }
        IStyleRepository Styles { get; }
        IServiceRequestRepository ServiceRequests { get; }
        ICartItemRepository CartItems { get; }
        IWishlistItemRepository WishlistItems { get; }
        IUserAddressRepository UserAddresses{ get; }
        IOrderRepository Orders { get; }
        IUserPurchaseTransactionRepository userPurchaseTransactions{ get; }
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();

    }
}
