using DataAccess.Context;
using DataAccess.IRepositories;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class StyleRepository : Repository<Style>, IStyleRepository
    {
        public StyleRepository(FPDbContext context) : base(context)
        {
        }
    }
}
