using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.IRepositories;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly FPDbContext _context;

        public ProductRepository(FPDbContext context) :base(context)
        {
            _context = context;

        }

        public async Task<List<Product>> GetAllWithIncludesAsync(paginationDto paginationDto)
        {
            var skip = (paginationDto.PageNumber - 1) * paginationDto.PageSize;
            var products = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Material)
            .Include(p => p.Style)
            .Include(p => p.Brand)
            .Include(p => p.Images).Skip(skip)
            .Take(paginationDto.PageSize)
            .ToListAsync();
            return products;
        }
    }
}
