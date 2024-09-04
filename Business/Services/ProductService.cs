using AutoMapper;
using Business.IServices;
using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.IRepositories;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
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
        public async Task<ResponseModel<bool>> CreateProduct(PostProdcutDto postProdcutDto)
        {
            var product = _mapper.Map<Product>(postProdcutDto);
            _unitOfWork.Products.AddAsync(product);
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
            return new ResponseModel<bool> {IsSuccess=true};
        }
        
        
        public async Task<ResponseModel<bool>> CreateProdcutBrand(PostBrandDto brandPostDto)
        {
            throw new NotImplementedException(); 
        }
        public async Task<ResponseModel<bool>> CreateProdcutStyle(PostStyleDto postStyleDto)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseModel<bool>> CreateProdcutCategory(PostCategoryDto postCategoryDto)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseModel<bool>> CreateProdcutMaterial(PostMaterialDto postMaterialDto)
        {
            throw new NotImplementedException();
        }
    }
}
