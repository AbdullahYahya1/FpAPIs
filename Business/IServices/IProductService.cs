using DataAccess.DTOs;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IProductService : IService<Product>
    {
        Task<ResponseModel<bool>> CreateProduct(PostProdcutDto postProdcutDto);
        Task<ResponseModel<bool>> CreateProdcutMaterial(PostMaterialDto postMaterialDto);
        Task<ResponseModel<bool>> CreateProdcutCategory(PostCategoryDto postCategoryDto);
        Task<ResponseModel<bool>> CreateProdcutStyle(PostStyleDto postStyleDto);
        Task<ResponseModel<bool>> CreateProdcutBrand(PostBrandDto brandPostDto);
    }
}
