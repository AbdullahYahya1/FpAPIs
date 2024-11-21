using Business.IServices;
using DataAccess.DTOs;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FpAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("SearchProducts")]
        public async Task<ActionResult<ResponseModel<List<GetProductDto>>>> SearchProducts([FromQuery] ProductSearchDto productSearchDto)
        {
            var res = await _productService.SearchProducts(productSearchDto);
            return Ok(res);
        }
        [Authorize]
        [HttpGet("GetProduct/{productId}")]
        public async Task<ActionResult<ResponseModel<GetProductDto>>> GetProducts(int productId)
        {
            var res = await _productService.GetProduct(productId);
            return Ok(res);
        }
        [Authorize]
        [HttpPost("AddProduct")]
        public async Task<ActionResult<ResponseModel<Product>>> AddProdcut(PostProdcutDto postProdcutDto)
        {
            var res = await _productService.CreateProduct(postProdcutDto);
            return Ok(res);
        }
        [Authorize]
        [HttpPut("UpdateProduct/{ProductId}")]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateProduct([FromRoute] int ProductId, PostProdcutDto updateProductDto)
        {
            var res = await _productService.UpdateProduct(ProductId, updateProductDto);
            return Ok(res);
        }
        [Authorize]
        [HttpPost("DeactivateProduct/{productId}")]
        public async Task<ActionResult<ResponseModel<bool>>> DeactivateProduct(int productId)
        {
            var res = await _productService.DeactivateProduct(productId);
            return Ok(res);
        }
        [Authorize]
        [HttpPost("AddBrand")]
        public async Task<ActionResult<ResponseModel<bool>>> AddBrand(PostBrandDto brandDto)
        {
            var res = await _productService.CreateProdcutBrand(brandDto);
            return Ok(res);
        }
        [Authorize]
        [HttpPost("AddStyle")]
        public async Task<ActionResult<ResponseModel<bool>>> AddStyle(PostStyleDto styleDto)
        {
            var res = await _productService.CreateProdcutStyle(styleDto);
            return Ok(res);
        }
        [Authorize]
        [HttpPost("AddMaterial")]
        public async Task<ActionResult<ResponseModel<bool>>> AddMaterial(PostMaterialDto materialDto)
        {
            var res = await _productService.CreateProdcutMaterial(materialDto);
            return Ok(res);
        }
        [Authorize]
        [HttpPost("AddCategory")]
        public async Task<ActionResult<ResponseModel<bool>>> AddCategory(PostCategoryDto categoryDto)
        {
            var res = await _productService.CreateProdcutCategory(categoryDto);
            return Ok(res);
        }
        [HttpGet("GetBrandsLookUp")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> GetBrandsLookUp()
        {
            var res = await _productService.GetBrandsLookUp();
            return Ok(res);
        }

        [HttpGet("GetStyleLookUp")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> GetStyleLookUp()
        {
            var res = await _productService.GetStyleLookUp();
            return Ok(res);
        }

        [HttpGet("GetMaterialLookUp")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> GetMaterialLookUp()
        {
            var res = await _productService.GetMaterialLookUp();
            return Ok(res);
        }

        [HttpGet("GetCategoryLookUp")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> GetCategoryLookUp()
        {
            var res = await _productService.GetCategoryLookUp();
            return Ok(res);
        }

        [HttpGet("GetCategories")]
        public async Task<ActionResult<ResponseModel<List<GetProductCatagorysDto>>>> GetCategories()
        {
            var res = await _productService.GetCategories();
            return Ok(res);
        }

        [HttpGet("ProductStatusLookup")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> ProductStatusLookup()
        {
            var res = await _productService.ProductStatusLookup();
            return Ok(res);
        }

        [HttpGet("ProductColorLookup")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> ProductColorLookup()
        {
            var res = await _productService.ProductColorLookup();
            return Ok(res);
        }
    }
}
