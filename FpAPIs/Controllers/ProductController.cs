using Business.IServices;
using Common;
using DataAccess.DTOs;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FpAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("SearchProducts")]
        public async Task<ActionResult<ResponseModel<List<GetProductDto>>>> SearchProducts([FromQuery] ProductSearchDto productSearchDto)
        {
            _logger.LogInformation($"Input received for SearchProducts: {System.Text.Json.JsonSerializer.Serialize(productSearchDto)}");

            var res = await _productService.SearchProducts(productSearchDto);

            _logger.LogInformation($"Output for SearchProducts: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpGet("GetProduct/{productId}")]
        public async Task<ActionResult<ResponseModel<GetProductDto>>> GetProducts(int productId)
        {
            _logger.LogInformation($"Input received for GetProduct: productId = {productId}");

            var res = await _productService.GetProduct(productId);

            _logger.LogInformation($"Output for GetProduct: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [Authorize]
        [CustomAuthorize([UserType.Manager])]
        [HttpPost("AddProduct")]
        public async Task<ActionResult<ResponseModel<Product>>> AddProdcut(PostProdcutDto postProdcutDto)
        {
            _logger.LogInformation($"Input received for AddProduct: {System.Text.Json.JsonSerializer.Serialize(postProdcutDto)}");

            var res = await _productService.CreateProduct(postProdcutDto);

            _logger.LogInformation($"Output for AddProduct: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [Authorize]
        [CustomAuthorize([UserType.Manager])]
        [HttpPut("UpdateProduct/{ProductId}")]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateProduct([FromRoute] int ProductId, PostProdcutDto updateProductDto)
        {
            _logger.LogInformation($"Input received for UpdateProduct: ProductId = {ProductId}, {System.Text.Json.JsonSerializer.Serialize(updateProductDto)}");

            var res = await _productService.UpdateProduct(ProductId, updateProductDto);

            _logger.LogInformation($"Output for UpdateProduct: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [Authorize]
        [CustomAuthorize([UserType.Manager])]
        [HttpPost("DeactivateProduct/{productId}")]
        public async Task<ActionResult<ResponseModel<bool>>> DeactivateProduct(int productId)
        {
            _logger.LogInformation($"Input received for DeactivateProduct: productId = {productId}");

            var res = await _productService.DeactivateProduct(productId);

            _logger.LogInformation($"Output for DeactivateProduct: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [Authorize]
        [CustomAuthorize([UserType.Manager])]
        [HttpPost("AddBrand")]
        public async Task<ActionResult<ResponseModel<bool>>> AddBrand(PostBrandDto brandDto)
        {
            _logger.LogInformation($"Input received for AddBrand: {System.Text.Json.JsonSerializer.Serialize(brandDto)}");

            var res = await _productService.CreateProdcutBrand(brandDto);

            _logger.LogInformation($"Output for AddBrand: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [Authorize]
        [CustomAuthorize([UserType.Manager])]
        [HttpPost("AddStyle")]
        public async Task<ActionResult<ResponseModel<bool>>> AddStyle(PostStyleDto styleDto)
        {
            _logger.LogInformation($"Input received for AddStyle: {System.Text.Json.JsonSerializer.Serialize(styleDto)}");

            var res = await _productService.CreateProdcutStyle(styleDto);

            _logger.LogInformation($"Output for AddStyle: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [Authorize]
        [CustomAuthorize([UserType.Manager])]
        [HttpPost("AddMaterial")]
        public async Task<ActionResult<ResponseModel<bool>>> AddMaterial(PostMaterialDto materialDto)
        {
            _logger.LogInformation($"Input received for AddMaterial: {System.Text.Json.JsonSerializer.Serialize(materialDto)}");

            var res = await _productService.CreateProdcutMaterial(materialDto);

            _logger.LogInformation($"Output for AddMaterial: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [Authorize]
        [CustomAuthorize([UserType.Manager])]
        [HttpPost("AddCategory")]
        public async Task<ActionResult<ResponseModel<bool>>> AddCategory(PostCategoryDto categoryDto)
        {
            _logger.LogInformation($"Input received for AddCategory: {System.Text.Json.JsonSerializer.Serialize(categoryDto)}");

            var res = await _productService.CreateProdcutCategory(categoryDto);

            _logger.LogInformation($"Output for AddCategory: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpGet("GetBrandsLookUp")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> GetBrandsLookUp()
        {
            _logger.LogInformation("Request received for GetBrandsLookUp.");

            var res = await _productService.GetBrandsLookUp();

            _logger.LogInformation($"Output for GetBrandsLookUp: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpGet("GetStyleLookUp")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> GetStyleLookUp()
        {
            _logger.LogInformation("Request received for GetStyleLookUp.");

            var res = await _productService.GetStyleLookUp();

            _logger.LogInformation($"Output for GetStyleLookUp: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpGet("GetMaterialLookUp")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> GetMaterialLookUp()
        {
            _logger.LogInformation("Request received for GetMaterialLookUp.");

            var res = await _productService.GetMaterialLookUp();

            _logger.LogInformation($"Output for GetMaterialLookUp: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpGet("GetCategoryLookUp")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> GetCategoryLookUp()
        {
            _logger.LogInformation("Request received for GetCategoryLookUp.");

            var res = await _productService.GetCategoryLookUp();

            _logger.LogInformation($"Output for GetCategoryLookUp: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpGet("GetCategories")]
        public async Task<ActionResult<ResponseModel<List<GetProductCatagorysDto>>>> GetCategories()
        {
            _logger.LogInformation("Request received for GetCategories.");

            var res = await _productService.GetCategories();

            _logger.LogInformation($"Output for GetCategories: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpGet("ProductStatusLookup")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> ProductStatusLookup()
        {
            _logger.LogInformation("Request received for ProductStatusLookup.");

            var res = await _productService.ProductStatusLookup();

            _logger.LogInformation($"Output for ProductStatusLookup: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpGet("ProductColorLookup")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> ProductColorLookup()
        {
            _logger.LogInformation("Request received for ProductColorLookup.");

            var res = await _productService.ProductColorLookup();

            _logger.LogInformation($"Output for ProductColorLookup: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }
    }
}
