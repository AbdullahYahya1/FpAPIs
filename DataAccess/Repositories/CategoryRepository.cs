using DataAccess.Context;
using DataAccess.IRepositories;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(FPDbContext context) : base(context)
        {
        }
    }
}
