using DataAccess.Context;
using DataAccess.IRepositories;

namespace DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FPDbContext _db;
        public IUserRepository Users { get; }
        public IProductRepository Products { get; }


        public UnitOfWork(FPDbContext context,  IUserRepository userRepository , IProductRepository ProductRepository)
        {
            _db = context;
            Users = userRepository;
            Products = ProductRepository;
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
