using DataAccess.Context;
using DataAccess.IRepositories;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        public MaterialRepository(FPDbContext context) : base(context)
        {
        }
    }
}
