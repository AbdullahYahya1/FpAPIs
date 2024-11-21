using Azure.Core;
using Business.IServices;
using DataAccess.DTOs;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public ServiceRequestController(IServiceRequestService serviceRequest)
        {
            _serviceRequest = serviceRequest;
        }

        [HttpGet("GetCurrentUserServices")]
        public async Task<ActionResult<ResponseModel<List<GetServiceDto>>>> GetCurrentUserServices()
        {
            var res = await _serviceRequest.GetServices();
            return Ok(res);
        }

        [HttpGet("GetCurrentUserService/{ServiceId}")]
        public async Task<ActionResult<ResponseModel<GetServiceDto>>> GetCurrentUserService(int ServiceId)
        {
            var res = await _serviceRequest.GetServicesByID(ServiceId);
            return Ok(res);
        }

        [HttpPost("CreateService")]
        public async Task<ActionResult<ResponseModel<bool>>> CreateService(PostServiceDto serviceDto)
        {
            var res = await _serviceRequest.CreateService(serviceDto);
            return Ok(res);
        }

        [HttpPut("ResponseToRequest/{RequestId}")]
        public async Task<ActionResult<ResponseModel<bool>>> ResponseToRequest(int RequestId, UpdateRequestDto updateRequestDto)
        {
            var res = await _serviceRequest.ResponseToRequest(RequestId, updateRequestDto);
            return Ok(res);
        }

        [HttpGet("ProductRequestTypeLookup")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> ProductRequestTypeLookup()
        {
            var res = await _serviceRequest.ProductRequestTypeLookup();
            return Ok(res);
        }

        [HttpGet("ProductServiceRequestStatusLookup")]
        public async Task<ActionResult<ResponseModel<List<LookUpDataModel<int>>>>> ProductServiceRequestStatusLookup()
        {
            var res = await _serviceRequest.ProductServiceRequestStatusLookup();
            return Ok(res);
        }
    }
}
