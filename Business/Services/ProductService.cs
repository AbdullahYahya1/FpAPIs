using AutoMapper;
using Azure.Core;
using Business.IServices;
using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.IRepositories;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
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
        public async Task<ResponseModel<GetProductDto>> CreateProduct(PostProdcutDto postProdcutDto)
        {
            try { 
                var product = _mapper.Map<Product>(postProdcutDto);
                await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.SaveChangesAsync();
                if (postProdcutDto.ImagesString64 != null && postProdcutDto.ImagesString64.Any())
                {
                    foreach (var img in postProdcutDto.ImagesString64)
                    {
                        var imageBytes = Convert.FromBase64String(img);
                        var uniqueFileName = $"{Guid.NewGuid()}.jpg";
                        var physicalPath = Path.Combine("wwwroot", "images", uniqueFileName);
                        await System.IO.File.WriteAllBytesAsync(physicalPath, imageBytes);
                        var relativeImagePath = Path.Combine("images", uniqueFileName).Replace("\\", "/");
                        var productImage = new ProductImage
                        {
                            ImageUrl = relativeImagePath,
                            ProductId = product.ProductId
                        };
                        product.Images.Add(productImage);
                    }
                    await _unitOfWork.Products.UpdateAsync(product); 
                    await _unitOfWork.SaveChangesAsync();

                }
                var productDto = _mapper.Map<GetProductDto>(product);
                return new ResponseModel<GetProductDto> { IsSuccess = true, Result = productDto };
            }catch(Exception ex)
            {
                return new ResponseModel<GetProductDto> { IsSuccess = false, Message = "ErrorFound" };
            }
        }
        public async Task<ResponseModel<GetProductDto>> UpdateProduct(int productId, PostProdcutDto updateProductDto)
        {
            try
            {
                var product = await _unitOfWork.Products.GetProductWithImageIncludesAsync(productId);
                if (product == null)
                {
                    return new ResponseModel<GetProductDto> { IsSuccess = false, Message = "ProductNotFound" };
                }

                _mapper.Map(updateProductDto, product);
                if (updateProductDto.ImagesString64 == null || updateProductDto.ImagesString64.Count() == 0)
                {
                    foreach (var image in product.Images)
                    {
                        var imagePath = Path.Combine("wwwroot", image.ImageUrl);
                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath);
                        }
                        _context.Set<ProductImage>().Remove(image);
                    }
                    await _unitOfWork.Products.UpdateAsync(product);
                    await _unitOfWork.SaveChangesAsync();
                }
                if (updateProductDto.ImagesString64 != null && updateProductDto.ImagesString64.Any())
                {
                    var currentImageUrls = product.Images.Select(img => img.ImageUrl).ToList();
                    var newBase64Images = new List<string>();
                    var updatedImageUrls = new List<string>();

                    
                    foreach (var imageString in updateProductDto.ImagesString64)
                    {
                        if (imageString.StartsWith("images/"))
                        {
                            updatedImageUrls.Add(imageString);
                        }
                        else
                        {
                            newBase64Images.Add(imageString);
                        }
                    }

                    
                    var imagesToRemove = product.Images
                        .Where(img => !updatedImageUrls.Contains(img.ImageUrl))
                        .ToList();

                    foreach (var image in imagesToRemove)
                    {
                        var imagePath = Path.Combine("wwwroot", image.ImageUrl);
                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath); 
                        }
                        _context.Set<ProductImage>().Remove(image); 
                    }

                    
                    foreach (var base64Image in newBase64Images)
                    {
                        try
                        {
                            var imageBytes = Convert.FromBase64String(base64Image);
                            var uniqueFileName = $"{Guid.NewGuid()}.jpg";
                            var physicalPath = Path.Combine("wwwroot", "images", uniqueFileName);
                            await System.IO.File.WriteAllBytesAsync(physicalPath, imageBytes);
                            var relativeImagePath = Path.Combine("images", uniqueFileName).Replace("\\", "/");
                            var productImage = new ProductImage
                            {
                                ImageUrl = relativeImagePath,
                                ProductId = product.ProductId
                            };
                            product.Images.Add(productImage);
                        }
                        catch (FormatException)
                        {
                            return new ResponseModel<GetProductDto> { IsSuccess = false, Message = "InvalidBase64Format" };
                        }
                    }
                }

                await _unitOfWork.Products.UpdateAsync(product);
                await _unitOfWork.SaveChangesAsync();
                var updatedProductDto = _mapper.Map<GetProductDto>(product);

                return new ResponseModel<GetProductDto> { IsSuccess = true, Result = updatedProductDto};
            }
            catch (Exception ex)
            { 
                return new ResponseModel<GetProductDto>
                {
                    Message = $"ErrorFound: {ex.Message}",
                    IsSuccess = false
                };
            }
        }

        public async Task<ResponseModel<bool>> CreateProdcutBrand(PostBrandDto brandPostDto)
        {
            try
            {
                var brand = _mapper.Map<Brand>(brandPostDto);
                await _unitOfWork.Brands.AddAsync(brand);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel<bool> { IsSuccess = true, Result = true };
            }
            catch (Exception ex) {
                return new ResponseModel<bool>
                {
                    Message = "ErrorFound",
                    IsSuccess = false

                };
            
            }
        }
        public async Task<ResponseModel<bool>> CreateProdcutStyle(PostStyleDto postStyleDto)
        {
            try { 
            var Style = _mapper.Map<Style>(postStyleDto);
            await _unitOfWork.Styles.AddAsync(Style);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel<bool> { IsSuccess = true, Result = true };
            }catch(Exception ex)
            {
                return new ResponseModel<bool>
                {
                    Message = "ErrorFound",
                    IsSuccess = false
                };
            }
}
        public async Task<ResponseModel<bool>> CreateProdcutCategory(PostCategoryDto postCategoryDto)
        {
            try
            {
                var Category = _mapper.Map<Category>(postCategoryDto);
                await _unitOfWork.Categorys.AddAsync(Category);
                await _unitOfWork.SaveChangesAsync();
                var imageBytes = Convert.FromBase64String(postCategoryDto.ImagesString64);
                var uniqueFileName = $"{Guid.NewGuid()}.jpg";
                var physicalPath = Path.Combine("wwwroot", "images", uniqueFileName);
                await System.IO.File.WriteAllBytesAsync(physicalPath, imageBytes);
                var relativeImagePath = Path.Combine("images", uniqueFileName).Replace("\\", "/");
                Category.ImageUrl = relativeImagePath;
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel<bool> { IsSuccess = true, Result = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>
                {
                    Message = "ErrorFound",
                    IsSuccess = false
                };
            }
        }
        public async Task<ResponseModel<bool>> CreateProdcutMaterial(PostMaterialDto postMaterialDto)
        {
                try
                {
                    var Material = _mapper.Map<Material>(postMaterialDto);
                    await _unitOfWork.Materials.AddAsync(Material);
                    await _unitOfWork.SaveChangesAsync();
                    return new ResponseModel<bool> { IsSuccess = true, Result = true };
                }
                catch (Exception ex) {
                    return new ResponseModel<bool>
                    {
                        Message = "ErrorFound",
                        IsSuccess = false
                    };
                }
        }

        public async Task<ResponseModel<List<LookUpDataModel<int>>>> GetBrandsLookUp()
        {
            try
            {
                var brands = await _unitOfWork.Brands.GetAllAsync();
                var lookup = brands.Select(brand => new LookUpDataModel<int>
                {
                    Value = brand.BrandId,
                    NameAr = brand.BrandName,
                    NameEn = brand.BrandName
                }).ToList();
                return new ResponseModel<List<LookUpDataModel<int>>> { Result = lookup, IsSuccess = true };
            }
            catch (Exception ex) {
                return new ResponseModel<List<LookUpDataModel<int>>>
                {
                    Result = new List<LookUpDataModel<int>>(),
                    IsSuccess = false,
                    Message = "ErrorFound"
                };

            }
        }

        public async Task<ResponseModel<List<LookUpDataModel<int>>>> GetStyleLookUp()
        {
            try
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
            catch (Exception ex) {
                return new ResponseModel<List<LookUpDataModel<int>>>
                {
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }

        public async Task<ResponseModel<List<LookUpDataModel<int>>>> GetMaterialLookUp()
        {
            try
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
            catch (Exception ex) {
                return new ResponseModel<List<LookUpDataModel<int>>>
                {
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }

        public async Task<ResponseModel<List<LookUpDataModel<int>>>> GetCategoryLookUp()
        {
            try
            {
                var materials = await _unitOfWork.Categorys.GetAllAsync();
                var lookup = materials.Select(Material => new LookUpDataModel<int>
                {
                    Value = Material.CategoryId,
                    NameAr = Material.NameAr,
                    NameEn = Material.NameEn
                }).ToList();
                return new ResponseModel<List<LookUpDataModel<int>>> { Result = lookup, IsSuccess = true };
            }
            catch (Exception ex) {
                return new ResponseModel<List<LookUpDataModel<int>>>
                {
                    Message = "ErrorFound",
                    IsSuccess = false
                };
            }
        }
        public async Task<ResponseModel<List<GetProductCatagorysDto>>> GetCategories()
        {
            try
            {
                var catagorys = await _unitOfWork.Categorys.GetAllAsync();
                var catagorysDto = _mapper.Map<List<GetProductCatagorysDto>>(catagorys);
                return new ResponseModel<List<GetProductCatagorysDto>> { IsSuccess = true, Result = catagorysDto };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetProductCatagorysDto>>
                {
                    Message = "ErrorFound",
                    IsSuccess = false
                };
            }
        }
        public async Task<ResponseModel<List<GetProductDto>>> GetProducts(paginationDto paginationDto)
        {
            try
            {
                var products = await _unitOfWork.Products.GetAllWithIncludesAsync(paginationDto);
                var productsDtos = _mapper.Map<List<GetProductDto>>(products);
                return new ResponseModel<List<GetProductDto>> { IsSuccess = true, Result = productsDtos };
            }
            catch (Exception ex) {
                return new ResponseModel<List<GetProductDto>>
                {
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }

        public async Task<ResponseModel<List<GetProductDto>>> SearchProducts(ProductSearchDto productSearchDto)
        {
            try
            {
                var products = await _unitOfWork.Products.SearchAsync(productSearchDto);
                var productsDtos = _mapper.Map<List<GetProductDto>>(products);
                return new ResponseModel<List<GetProductDto>> { IsSuccess = true, Result = productsDtos };
            }
            catch (Exception ex) {
                return new ResponseModel<List<GetProductDto>>
                {
                    Message = "ErrorFound",
                    IsSuccess = false
                };
            }
        }

        public async Task<ResponseModel<GetProductDto>> GetProduct(int productId)
        {
            try
            {
            var product = await _unitOfWork.Products.GetProductAllWithIncludesAsync(productId);
            var productdto = _mapper.Map<GetProductDto>(product);
            return new ResponseModel<GetProductDto> { IsSuccess = true, Result = productdto };
            }
            catch (Exception ex)
            {
                return new ResponseModel<GetProductDto>
                {
                    Message = "ErrorFound",
                    IsSuccess = false
                };
            }
        }


        public async Task<ResponseModel<List<LookUpDataModel<int>>>> ProductStatusLookup()
        {
            try
            {
                var result = Enum.GetValues(typeof(ProductStatus))
                                 .Cast<ProductStatus>()
                                 .Select(enumValue => new LookUpDataModel<int>
                                 {
                                     Value = Convert.ToInt32(enumValue),
                                     NameAr = enumValue.ToString(),
                                     NameEn = enumValue.ToString()
                                 }).ToList();
                return new ResponseModel<List<LookUpDataModel<int>>> { Result = result, IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<LookUpDataModel<int>>>
                {
                    Result = new List<LookUpDataModel<int>>(),
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }
        public async Task<ResponseModel<List<LookUpDataModel<int>>>> ProductColorLookup()
        {
            try
            {
                var result = Enum.GetValues(typeof(Color))
                                 .Cast<Color>()
                                 .Select(enumValue => new LookUpDataModel<int>
                                 {
                                     Value = Convert.ToInt32(enumValue),
                                     NameAr = enumValue.ToString(),
                                     NameEn = enumValue.ToString()
                                 }).ToList();
                return new ResponseModel<List<LookUpDataModel<int>>> { Result = result, IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<LookUpDataModel<int>>>
                {
                    Result = new List<LookUpDataModel<int>>(),
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }

        public async Task<ResponseModel<bool>> DeactivateProduct(int productId)
        {
            try
            {
                var product = await _unitOfWork.Products.GetProductAllWithIncludesAsync(productId);
                if (product == null)
                {
                    return new ResponseModel<bool> { IsSuccess = false, Message = "ProductNotFound" };
                }
                if (product.ProductStatus != ProductStatus.Active)
                {
                    return new ResponseModel<bool> { IsSuccess = false, Message = "ProductIsNotActive" };
                }
                foreach (var item in product.CartItems)
                {
                    product.CartItems.Remove(item);
                }
                foreach (var item in product.WishlistItems)
                {
                    product.WishlistItems.Remove(item);
                }
                product.ProductStatus = ProductStatus.Inactive;
                await _unitOfWork.Products.UpdateAsync(product);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel<bool> { IsSuccess = true, Result = true };
            }catch (Exception ex)
            {
                return new ResponseModel<bool>
                {
                    Message = "ErrorFound",
                    IsSuccess = false
                };
            }
        }

  
    }
}
