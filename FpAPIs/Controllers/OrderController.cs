using Business.IServices;
using Common;
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
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("GetOrders")]
        [CustomAuthorize([UserType.Manager,UserType.Client,UserType.DeliveryRepresentative])]

        public async Task<ActionResult<ResponseModel<List<GetOrderDto>>>> GetOrders()
        {
            var res = await _orderService.GetOrders();
            return Ok(res);
        }

        [HttpPost("CreateOrder")]
        [CustomAuthorize([UserType.Client])]

        public async Task<ActionResult<ResponseModel<GetOrderDto>>> CreateOrder(PostOrderDto postOrder)
        {
            var res = await _orderService.AddOrder(postOrder);
            return Ok(res);
        }

        [HttpPost("Pay/{OrderId}")]
        [CustomAuthorize([UserType.Client])]
        public async Task<ActionResult<ResponseModel<GetOrderDto>>> Pay([FromRoute] int OrderId, [FromBody] PayOrderDto payOrderDto)
        {
            var res = await _orderService.PayOrder(OrderId, payOrderDto);
            return Ok(res);
        }

        [HttpPost("AssignDriver/{UserId}/Order/{OrderId}")]
        [CustomAuthorize([UserType.Manager])]
        public async Task<ActionResult<ResponseModel<bool>>> AssignDriver([FromRoute] int OrderId, [FromRoute] string UserId)
        {
            var res = await _orderService.AssignDriver(OrderId, UserId);
            return Ok(res);
        }
        [HttpPost("Driver/Deliver/{OrderId}")]
        [CustomAuthorize([UserType.DeliveryRepresentative])]

        public async Task<ActionResult<ResponseModel<bool>>> DeliverOrder([FromRoute] int OrderId)
        {
            var res = await _orderService.DeliverOrder(OrderId);
            return Ok(res);
        }
        [HttpPost("Driver/Cancel/{OrderId}")]
        [CustomAuthorize([UserType.DeliveryRepresentative])]

        public async Task<ActionResult<ResponseModel<bool>>> CancelOrder([FromRoute] int OrderId)
        {
            var res = await _orderService.CancelOrder(OrderId);
            return Ok(res);
        }
        [HttpPost("Driver/PickUp/{OrderId}")]
        [CustomAuthorize([UserType.DeliveryRepresentative])]
        public async Task<ActionResult<ResponseModel<bool>>> PickUp([FromRoute] int OrderId)
        {
            var res = await _orderService.PickUpOrder(OrderId);
            return Ok(res);
        }
    }
}
