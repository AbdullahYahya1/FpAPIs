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
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet("GetOrders")]
        [CustomAuthorize([UserType.Manager, UserType.Client, UserType.DeliveryRepresentative])]
        public async Task<ActionResult<ResponseModel<List<GetOrderDto>>>> GetOrders()
        {
            _logger.LogInformation("Request received for GetOrders.");

            var res = await _orderService.GetOrders();

            _logger.LogInformation($"Output for GetOrders: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpPost("CreateOrder")]
        [CustomAuthorize([UserType.Client])]
        public async Task<ActionResult<ResponseModel<GetOrderDto>>> CreateOrder(PostOrderDto postOrder)
        {
            _logger.LogInformation($"Input received for CreateOrder: {System.Text.Json.JsonSerializer.Serialize(postOrder)}");

            var res = await _orderService.AddOrder(postOrder);

            _logger.LogInformation($"Output for CreateOrder: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpPost("Pay/{OrderId}")]
        [CustomAuthorize([UserType.Client])]
        public async Task<ActionResult<ResponseModel<GetOrderDto>>> Pay([FromRoute] int OrderId, [FromBody] PayOrderDto payOrderDto)
        {
            _logger.LogInformation($"Input received for Pay: OrderId = {OrderId}, {System.Text.Json.JsonSerializer.Serialize(payOrderDto)}");

            var res = await _orderService.PayOrder(OrderId, payOrderDto);

            _logger.LogInformation($"Output for Pay: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpPost("AssignDriver/{UserId}/Order/{OrderId}")]
        [CustomAuthorize([UserType.Manager])]
        public async Task<ActionResult<ResponseModel<bool>>> AssignDriver([FromRoute] int OrderId, [FromRoute] string UserId)
        {
            _logger.LogInformation($"Input received for AssignDriver: OrderId = {OrderId}, UserId = {UserId}");

            var res = await _orderService.AssignDriver(OrderId, UserId);

            _logger.LogInformation($"Output for AssignDriver: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpPost("Driver/Deliver/{OrderId}")]
        [CustomAuthorize([UserType.DeliveryRepresentative])]
        public async Task<ActionResult<ResponseModel<bool>>> DeliverOrder([FromRoute] int OrderId)
        {
            _logger.LogInformation($"Input received for DeliverOrder: OrderId = {OrderId}");

            var res = await _orderService.DeliverOrder(OrderId);

            _logger.LogInformation($"Output for DeliverOrder: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpPost("Driver/Cancel/{OrderId}")]
        [CustomAuthorize([UserType.DeliveryRepresentative])]
        public async Task<ActionResult<ResponseModel<bool>>> CancelOrder([FromRoute] int OrderId)
        {
            _logger.LogInformation($"Input received for CancelOrder: OrderId = {OrderId}");

            var res = await _orderService.CancelOrder(OrderId);

            _logger.LogInformation($"Output for CancelOrder: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpPost("Driver/PickUp/{OrderId}")]
        [CustomAuthorize([UserType.DeliveryRepresentative])]
        public async Task<ActionResult<ResponseModel<bool>>> PickUp([FromRoute] int OrderId)
        {
            _logger.LogInformation($"Input received for PickUp: OrderId = {OrderId}");

            var res = await _orderService.PickUpOrder(OrderId);

            _logger.LogInformation($"Output for PickUp: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }
    }
}
