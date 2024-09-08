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

        Task<int> SaveChangesAsync();

    }
}
