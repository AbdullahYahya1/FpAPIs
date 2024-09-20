using DataAccess.Context;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FPDbContext _db;
        public IUserRepository Users { get; }
        public IProductRepository Products { get; }

        public IBrandRepository Brands { get; }
        public IMaterialRepository Materials { get; }
        public ICategoryRepository Categorys { get; }
        public IStyleRepository Styles { get; }
        public ICartItemRepository CartItems { get; }
        public IWishlistItemRepository WishlistItems { get; }
        public IServiceRequestRepository ServiceRequests { get; }

        public IUserAddressRepository UserAddresses { get; }

        public IOrderRepository Orders { get; }

        public IUserPurchaseTransactionRepository userPurchaseTransactions { get; }

        public UnitOfWork(
            FPDbContext context,
            IUserRepository userRepository,
            IProductRepository productRepository,
            IBrandRepository brandRepository,
            IMaterialRepository materialRepository,
            ICategoryRepository categoryRepository,
            IStyleRepository styleRepository,
            IServiceRequestRepository serviceRequests,
            ICartItemRepository cartItemRepository,
            IWishlistItemRepository wishlistItemRepository,
            IUserAddressRepository userAddressRepository,
            IOrderRepository orderRepository
            ,
            IUserPurchaseTransactionRepository userPurchaseTransactionsRepository)
        {
            _db = context;
            Users = userRepository;
            Products = productRepository;
            Brands = brandRepository;
            Materials = materialRepository;
            Categorys = categoryRepository;
            Styles = styleRepository;
            ServiceRequests = serviceRequests;
            CartItems = cartItemRepository;
            WishlistItems = wishlistItemRepository;
            UserAddresses = userAddressRepository;
            Orders = orderRepository;
            userPurchaseTransactions = userPurchaseTransactionsRepository;
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _db.Database.BeginTransactionAsync();
        }
    }
}
