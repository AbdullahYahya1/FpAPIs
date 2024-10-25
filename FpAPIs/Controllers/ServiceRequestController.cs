using Azure.Core;
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
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServiceRequestService _serviceRequest;

        public ServiceRequestController(IServiceRequestService serviceRequest) {
            _serviceRequest = serviceRequest;
        }
        [HttpGet("GetCurrentUserServices")]
        public async Task<IActionResult> GetCurrentUserServices()
        {
            var res = await _serviceRequest.GetServices();
            return Ok(res);
        }
        [HttpGet("GetCurrentUserService/{ServiceId}")]
        public async Task<IActionResult> GetCurrentUserService(int ServiceId)
        {
            var res = await _serviceRequest.GetServicesByID(ServiceId);
            return Ok(res);
        }
        [HttpPost("CreateService")]
        public async Task<IActionResult> CreateService(PostServiceDto serviceDto)
        {
            var res = await _serviceRequest.CreateService(serviceDto);
            return Ok(res); 
        }
        [HttpPut("ResponseToRequest/{RequestId}")]
        public async Task<IActionResult> ResponseToRequest(int RequestId, UpdateRequestDto updateRequestDto)
        {
            var res = await _serviceRequest.ResponseToRequest(RequestId, updateRequestDto);
            return Ok(res);
        }

        [HttpGet("ProductRequestTypeLookup")]
        public async Task<IActionResult> ProductRequestTypeLookup()
        {
            var res = await _serviceRequest.ProductRequestTypeLookup();
            return Ok(res);
        }

        [HttpGet("ProductServiceRequestStatusLookup")]
        public async Task<IActionResult> ProductServiceRequestStatusLookup()
        {
            var res = await _serviceRequest.ProductServiceRequestStatusLookup();
            return Ok(res);
        }
    }
}
