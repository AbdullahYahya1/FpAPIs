using DataAccess.Context;
using DataAccess.IRepositories;

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

        public IServiceRequestRepository ServiceRequests { get; }
        public UnitOfWork(
            FPDbContext context,
            IUserRepository userRepository,
            IProductRepository productRepository,
            IBrandRepository brandRepository,
            IMaterialRepository materialRepository,
            ICategoryRepository categoryRepository,
            IStyleRepository styleRepository,
            IServiceRequestRepository serviceRequests)
        {
            _db = context;
            Users = userRepository;
            Products = productRepository;
            Brands = brandRepository;
            Materials = materialRepository;
            Categorys = categoryRepository;
            Styles = styleRepository;
            ServiceRequests = serviceRequests;
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
