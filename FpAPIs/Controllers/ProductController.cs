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
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok();
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
    }
}
