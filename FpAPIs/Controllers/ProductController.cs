using Business.IServices;
using Business.Services;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FpAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService) {
            _productService = productService;
        }
        [HttpGet("SearchProducts")]
        public async Task<IActionResult> SearchProducts([FromQuery] ProductSearchDto productSearchDto)
        {
            var res = await _productService.SearchProducts(productSearchDto);
            return Ok(res); 
        }

        [HttpGet("GetProduct/{productId}")]
        public async Task<IActionResult> GetProducts(int productId)
        {
            var res = await _productService.GetProduct(productId);
            return Ok(res);
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProdcut(PostProdcutDto postProdcutDto)
        {
            var res = await _productService.CreateProduct(postProdcutDto);
            return Ok(res);
        }
        [HttpPut("UpdateProduct/{ProductId}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int ProductId, PostProdcutDto updateProductDto)
        {
            var res = await _productService.UpdateProduct(ProductId, updateProductDto);
            return Ok(res);
        }
        [HttpPost("DeactivateProduct/{productId}")]
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            var res = await _productService.Delete(productId);
            return Ok(res);
        }
        [HttpPost("AddBrand")]
        public async Task<IActionResult> AddBrand(PostBrandDto brandDto)
        {
            var res = await _productService.CreateProdcutBrand(brandDto);
            return Ok(res);
        }
        [HttpPost("AddStyle")]
        public async Task<IActionResult> AddStyle(PostStyleDto StyleDto)
        {
            var res = await _productService.CreateProdcutStyle(StyleDto);
            return Ok(res);
        }
        [HttpPost("AddMaterial")]
        public async Task<IActionResult> AddMaterial(PostMaterialDto MaterialDto)
        {
            var res = await _productService.CreateProdcutMaterial(MaterialDto);
            return Ok(res);
        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(PostCategoryDto CategoryDto)
        {
            var res = await _productService.CreateProdcutCategory(CategoryDto);
            return Ok(res);
        }

        [HttpGet("GetBrandsLookUp")]
        public async Task<IActionResult> GetBrandsLookUp()
        {
            var res = await _productService.GetBrandsLookUp();
            return Ok(res);
        }

        [HttpGet("GetStyleLookUp")]
        public async Task<IActionResult> GetStyleLookUp()
        {
            var res = await _productService.GetStyleLookUp();
            return Ok(res);
        }

        [HttpGet("GetMaterialLookUp")]
        public async Task<IActionResult> GetMaterialLookUp()
        {
            var res = await _productService.GetMaterialLookUp();
            return Ok(res);
        }

        [HttpGet("GetCategoryLookUp")]
        public async Task<IActionResult> GetCategoryLookUp()
        {
            var res = await _productService.GetCategoryLookUp();
            return Ok(res);
        }

        [HttpGet("ProductStatusLookup")]
        public async Task<IActionResult> ProductStatusLookup()
        {
            var res = await _productService.ProductStatusLookup();
            return Ok(res);
        }
        [HttpGet("ProductColorLookup")]
        public async Task<IActionResult> ProductColorLookup()
        {
            var res = await _productService.ProductColorLookup();
            return Ok(res);
        }
    }
}
