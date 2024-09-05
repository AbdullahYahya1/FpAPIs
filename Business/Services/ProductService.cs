using AutoMapper;
using Azure.Core;
using Business.IServices;
using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.IRepositories;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductService : Service<Product>, IProductService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(FPDbContext context , IMapper mapper , IUnitOfWork unitOfWork) : base(context)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork; 
        }
        public async Task<ResponseModel<Product>> CreateProduct(PostProdcutDto postProdcutDto)
        {
            var product = _mapper.Map<Product>(postProdcutDto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            if (postProdcutDto.ImagesString64 != null && postProdcutDto.ImagesString64.Any())
            {
                foreach (var img in postProdcutDto.ImagesString64)
                {
                    var imageBytes = Convert.FromBase64String(img);
                    var uniqueFileName = $"{Guid.NewGuid()}.jpg";
                    var imagePath = Path.Combine("wwwroot", "images", uniqueFileName);
                    await System.IO.File.WriteAllBytesAsync(imagePath, imageBytes);
                    var productImage = new ProductImage
                    {
                        ImageUrl = imagePath,
                        ProductId = product.ProductId
                    };
                    product.Images.Add(productImage);
                }
                await _unitOfWork.Products.UpdateAsync(product);
                await _unitOfWork.SaveChangesAsync();
            }
            
            return new ResponseModel<Product> {IsSuccess=true , Result= product };
        }
        
        
        public async Task<ResponseModel<bool>> CreateProdcutBrand(PostBrandDto brandPostDto)
        {
            var brand = _mapper.Map<Brand>(brandPostDto);
            await _unitOfWork.Brands.AddAsync(brand);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel<bool> { IsSuccess=true , Result= true};
        }
        public async Task<ResponseModel<bool>> CreateProdcutStyle(PostStyleDto postStyleDto)
        {
            var Style = _mapper.Map<Style>(postStyleDto);
            await _unitOfWork.Styles.AddAsync(Style);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel<bool> { IsSuccess = true, Result = true };
        }
        public async Task<ResponseModel<bool>> CreateProdcutCategory(PostCategoryDto postCategoryDto)
        {
            var Category = _mapper.Map<Category>(postCategoryDto);
            await _unitOfWork.Categorys.AddAsync(Category);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel<bool> { IsSuccess = true, Result = true };
        }
        public async Task<ResponseModel<bool>> CreateProdcutMaterial(PostMaterialDto postMaterialDto)
        {
            var Material = _mapper.Map<Material>(postMaterialDto);
            await _unitOfWork.Materials.AddAsync(Material);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel<bool> { IsSuccess = true, Result = true };
        }

        public async Task<ResponseModel<List<LookUpDataModel<int>>>> GetBrandsLookUp()
        {
            var brands = await _unitOfWork.Brands.GetAllAsync();
            var lookup = brands.Select(brand => new LookUpDataModel<int>
            {
                Value=brand.BrandId,
                NameAr = brand.BrandName,
                NameEn= brand.BrandName
            }).ToList();
            return new ResponseModel<List<LookUpDataModel<int>>> { Result = lookup , IsSuccess = true };
        }

        public async Task<ResponseModel<List<LookUpDataModel<int>>>> GetStyleLookUp()
        {
            var styles = await _unitOfWork.Styles.GetAllAsync();
            var lookup = styles.Select(brand => new LookUpDataModel<int>
            {
                Value = brand.StyleId,
                NameAr = brand.StyleNameAr,
                NameEn = brand.StyleNameEn
            }).ToList();
            return new ResponseModel<List<LookUpDataModel<int>>> { Result = lookup, IsSuccess = true };
        }

        public async Task<ResponseModel<List<LookUpDataModel<int>>>> GetMaterialLookUp()
        {
            var materials = await _unitOfWork.Materials.GetAllAsync();
            var lookup = materials.Select(Material => new LookUpDataModel<int>
            {
                Value = Material.MaterialId,
                NameAr = Material.MaterialNameAr,
                NameEn = Material.MaterialNameEn
            }).ToList();
            return new ResponseModel<List<LookUpDataModel<int>>> { Result = lookup, IsSuccess = true };
        }

        public async Task<ResponseModel<List<LookUpDataModel<int>>>> GetCategoryLookUp()
        {
            var materials =await _unitOfWork.Categorys.GetAllAsync();
            var lookup = materials.Select(Material => new LookUpDataModel<int>
            {
                Value = Material.CategoryId,
                NameAr = Material.NameAr,
                NameEn = Material.NameEn
            }).ToList();
            return new ResponseModel<List<LookUpDataModel<int>>> { Result = lookup, IsSuccess = true };
        }

        public async Task<ResponseModel<List<GetProductDto>>> GetProducts(paginationDto paginationDto)
        {
            var products = await _unitOfWork.Products.GetAllWithIncludesAsync(paginationDto);
            var productsDtos = _mapper.Map<List<GetProductDto>>(products);
            return new ResponseModel<List<GetProductDto>> { IsSuccess = true, Result = productsDtos }; 
        }
    }
}
