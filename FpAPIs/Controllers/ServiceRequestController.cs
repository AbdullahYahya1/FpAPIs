using Azure.Core;
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
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServiceRequestService _serviceRequest;
        private readonly ILogger<ServiceRequestController> _logger;

        public ServiceRequestController(IServiceRequestService serviceRequest, ILogger<ServiceRequestController> logger)
        {
            _serviceRequest = serviceRequest;
            _logger = logger;
        }

        [HttpGet("GetCurrentUserServices")]
        [CustomAuthorize([UserType.Manager, UserType.Client])]
        public async Task<ActionResult<ResponseModel<List<GetServiceDto>>>> GetCurrentUserServices()
        {
            _logger.LogInformation("Request received for GetCurrentUserServices.");

            var res = await _serviceRequest.GetServices();

            _logger.LogInformation($"Output for GetCurrentUserServices: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpGet("GetCurrentUserService/{ServiceId}")]
        [CustomAuthorize([UserType.Manager, UserType.Client])]
        public async Task<ActionResult<ResponseModel<GetServiceDto>>> GetCurrentUserService(int ServiceId)
        {
            _logger.LogInformation($"Request received for GetCurrentUserService with ServiceId = {ServiceId}.");

            var res = await _serviceRequest.GetServicesByID(ServiceId);

            _logger.LogInformation($"Output for GetCurrentUserService: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpPost("CreateService")]
        [CustomAuthorize([UserType.Client])]
        public async Task<ActionResult<ResponseModel<bool>>> CreateService(PostServiceDto serviceDto)
        {
            _logger.LogInformation($"Input received for CreateService: {System.Text.Json.JsonSerializer.Serialize(serviceDto)}");

            var res = await _serviceRequest.CreateService(serviceDto);

            _logger.LogInformation($"Output for CreateService: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpPut("ResponseToRequest/{RequestId}")]
        [CustomAuthorize([UserType.Manager])]
        public async Task<ActionResult<ResponseModel<bool>>> ResponseToRequest(int RequestId, UpdateRequestDto updateRequestDto)
        {
            _logger.LogInformation($"Input received for ResponseToRequest: RequestId = {RequestId}, {System.Text.Json.JsonSerializer.Serialize(updateRequestDto)}");

            var res = await _serviceRequest.ResponseToRequest(RequestId, updateRequestDto);

            _logger.LogInformation($"Output for ResponseToRequest: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpGet("ProductRequestTypeLookup")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> ProductRequestTypeLookup()
        {
            _logger.LogInformation("Request received for ProductRequestTypeLookup.");

            var res = await _serviceRequest.ProductRequestTypeLookup();

            _logger.LogInformation($"Output for ProductRequestTypeLookup: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }

        [HttpGet("ProductServiceRequestStatusLookup")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> ProductServiceRequestStatusLookup()
        {
            _logger.LogInformation("Request received for ProductServiceRequestStatusLookup.");

            var res = await _serviceRequest.ProductServiceRequestStatusLookup();

            _logger.LogInformation($"Output for ProductServiceRequestStatusLookup: {System.Text.Json.JsonSerializer.Serialize(res)}");
            return Ok(res);
        }
    }
}
