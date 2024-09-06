using DataAccess.DTOs;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetAllWithIncludesAsync(paginationDto paginationDto);
        Task<List<Product>> SearchAsync(ProductSearchDto productSearchDto);
        Task<Product> GetProductAllWithIncludesAsync(int productId);
    }
}
