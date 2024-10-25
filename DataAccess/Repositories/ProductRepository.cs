using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.IRepositories;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly FPDbContext _context;

        public ProductRepository(FPDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Product> GetProductAllWithIncludesAsync(int productId)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Material)
                .Include(p => p.Style)
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p=>p.ProductId == productId);
            return product;
        }
        public async Task<Product> GetProductWithImageIncludesAsync(int productId)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
            return product;
        }
        public async Task<List<Product>> GetAllWithIncludesAsync(paginationDto paginationDto)
        {
            var skip = (paginationDto.PageNumber - 1) * paginationDto.PageSize;
            var products = await _context.Products
                .AsNoTracking()  
                .Include(p => p.Category)
                .Include(p => p.Material)
                .Include(p => p.Style)
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .Skip(skip)
                .Take(paginationDto.PageSize)
                .Where(p=>p.ProductStatus == ProductStatus.Active)
                .ToListAsync();

            return products;
        }

        public async Task<List<Product>> SearchAsync(ProductSearchDto productSearchDto)
        {
            var query = _context.Products
                .AsNoTracking()  
                .Include(p => p.Category)
                .Include(p => p.Material)
                .Include(p => p.Style)
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .Where(p => p.ProductStatus == ProductStatus.Active)
                .AsQueryable();

            if (!string.IsNullOrEmpty(productSearchDto.Name))
            {
                query = query.Where(p => p.NameEn.ToLower().Contains(productSearchDto.Name.ToLower()) ||
                                         p.NameAr.ToLower().Contains(productSearchDto.Name.ToLower()));
            }
            if (productSearchDto.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == productSearchDto.CategoryId.Value);
            }

            if (productSearchDto.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= productSearchDto.MinPrice.Value);
            }

            if (productSearchDto.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= productSearchDto.MaxPrice.Value);
            }

            if (productSearchDto.Color.HasValue)
            {
                query = query.Where(p => p.Color == productSearchDto.Color.Value);
            }

            if (productSearchDto.BrandId.HasValue)
            {
                query = query.Where(p => p.BrandId == productSearchDto.BrandId.Value);
            }

            if (productSearchDto.MaterialId.HasValue)
            {
                query = query.Where(p => p.MaterialId == productSearchDto.MaterialId.Value);
            }

            if (productSearchDto.StyleId.HasValue)
            {
                query = query.Where(p => p.StyleId == productSearchDto.StyleId.Value);
            }

            if (productSearchDto.ProductStatus.HasValue)
            {
                query = query.Where(p => p.ProductStatus == productSearchDto.ProductStatus.Value);
            }

            if (!string.IsNullOrEmpty(productSearchDto.SortBy))
            {
                switch (productSearchDto.SortBy.ToLower())
                {
                    case "price":
                        query = productSearchDto.IsDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                        break;
                    case "name":
                        query = productSearchDto.IsDescending ? query.OrderByDescending(p => p.NameEn) : query.OrderBy(p => p.NameEn);
                        break;
                    default:
                        query = query.OrderBy(p => p.NameEn);
                        break;
                }
            }

            var skip = (productSearchDto.PageNumber - 1) * productSearchDto.PageSize;
            query = query.Skip(skip).Take(productSearchDto.PageSize);
            var products = await query.ToListAsync();

            return products;
        }
    }
}
