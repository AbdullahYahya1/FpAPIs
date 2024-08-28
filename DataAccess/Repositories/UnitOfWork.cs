using DataAccess.Context;
using DataAccess.IRepositories;

namespace DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WADbContext _db;
        public IUserRepository users { get; }
 
        public UnitOfWork(WADbContext context,  IUserRepository userRepository)
        {
            _db = context;
            users = userRepository;
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
