using DataAccess.Context;
using DataAccess.IRepositories;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(FPDbContext context) : base(context)
        {
        }
    }
}
