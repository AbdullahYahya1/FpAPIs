using Business.IServices;
using Business.Services;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FpAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService) {
            _orderService = orderService;
        }
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders() {
            var res = await _orderService.GetOrders();
            return Ok(res);
        }
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(PostOrderDto postOrder)
        {
            var res = await _orderService.AddOrder(postOrder); 
            return Ok(res);
        }
        [HttpPost("Pay/{OrderId}")]
        public async Task<IActionResult> Pay( [FromRoute] int OrderId , [FromBody] PayOrderDto payOrderDto)
        {
            var res = await _orderService.PayOrder(OrderId, payOrderDto);
            return Ok(res);
        }
    }
}
