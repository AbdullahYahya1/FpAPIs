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
        Task<ResponseModel<Product>> CreateProduct(PostProdcutDto postProdcutDto);

        Task<ResponseModel<List<GetProductDto>>> GetProducts(paginationDto paginationDto);
        Task<ResponseModel<GetProductDto>> GetProduct(int productId);

        Task<ResponseModel<bool>> CreateProdcutMaterial(PostMaterialDto postMaterialDto);
        Task<ResponseModel<bool>> CreateProdcutCategory(PostCategoryDto postCategoryDto);
        Task<ResponseModel<bool>> CreateProdcutStyle(PostStyleDto postStyleDto);
        Task<ResponseModel<bool>> CreateProdcutBrand(PostBrandDto brandPostDto);

        Task<ResponseModel<List<LookUpDataModel<int>>>> GetBrandsLookUp();
        Task<ResponseModel<List<LookUpDataModel<int>>>> GetStyleLookUp();
        Task<ResponseModel<List<LookUpDataModel<int>>>> GetMaterialLookUp();
        Task<ResponseModel<List<LookUpDataModel<int>>>> GetCategoryLookUp();
        Task<ResponseModel<List<GetProductDto>>> SearchProducts(ProductSearchDto productSearchDto);
        Task<ResponseModel<List<LookUpDataModel<int>>>> ProductStatusLookup();
        Task<ResponseModel<List<LookUpDataModel<int>>>> ProductColorLookup();
    }
}
