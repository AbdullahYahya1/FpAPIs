using AutoMapper;
using Business.IServices;
using Common;
using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.IRepositories;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ServiceRequestService : Service<ServiceRequest>, IServiceRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        public ServiceRequestService(FPDbContext context, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,INotificationService notificationService) : base(context)
        { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;

        }

        public async Task<ResponseModel<bool>> CreateService(PostServiceDto serviceRequest)
        {
            try {
                var service = _mapper.Map<ServiceRequest>(serviceRequest);

                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
                service.CreatedById = currentUserId;
                service.ServiceRequestStatus = ServiceRequestStatus.New;
                service.SubmissionDate = DateTime.Now;

                await _unitOfWork.ServiceRequests.AddAsync(service);
                await _unitOfWork.SaveChangesAsync();
                if (serviceRequest.ImagesString64 != null && serviceRequest.ImagesString64.Any())
                {
                    foreach (var img in serviceRequest.ImagesString64)
                    {
                        var imageBytes = Convert.FromBase64String(img);
                        var uniqueFileName = $"{Guid.NewGuid()}.jpg";
                        var physicalPath = Path.Combine("wwwroot", "images", uniqueFileName);
                        await System.IO.File.WriteAllBytesAsync(physicalPath, imageBytes);

                        var relativeImagePath = Path.Combine("images", uniqueFileName).Replace("\\", "/");

                        var serviceImage = new ServiceImage
                        {
                            ImageUrl = relativeImagePath,
                            RequestId = service.RequestId
                        };

                        service.Images.Add(serviceImage);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }
                await _notificationService.NotifyAdminAsync("Service");
                return new ResponseModel<bool> { IsSuccess = true, Result = true };
            } catch (Exception ex)
            {
                return new ResponseModel<bool> { IsSuccess = false, Message = "ErrorFound" };
            }
        }

        public async Task<ResponseModel<List<GetServiceDto>>> GetServices()
        {
            try { 
            var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
            var type = _httpContextAccessor.HttpContext.User?.FindFirst("UserType")?.Value;
            var services = new List<ServiceRequest>();
            if (type == UserType.Manager.ToString()) {
                 services = await _unitOfWork.ServiceRequests.GetAllWithImgsAsync();
                }
                else {
                 services = await _unitOfWork.ServiceRequests.GetAllWithImgsAsyncByUserId(currentUserId);
            }
            var servicesDto =_mapper.Map<List<GetServiceDto>>(services);
            return new ResponseModel<List<GetServiceDto>> { IsSuccess = true, Result = servicesDto };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetServiceDto>> { IsSuccess = false, Message = "ErrorFound" };
            }
        }

        public async Task<ResponseModel<GetServiceDto>> GetServicesByID(int Id)
        {
            try { 
            var service= await _unitOfWork.ServiceRequests.GetServiceWithImgsByServiceId(Id);
            var user = await _unitOfWork.Users.getUserById(service.CreatedById);
            var serviceDto = _mapper.Map<GetServiceDto>(service);
            serviceDto.UserPhone = user.MobileNumber;
                return new ResponseModel<GetServiceDto> { Result = serviceDto, IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel<GetServiceDto> { IsSuccess = false, Message = "ErrorFound" };
            }
        }

        public async Task<ResponseModel<List<LookUpDataModel<int>>>> ProductRequestTypeLookup()
        {
            try
            {
                var result = Enum.GetValues(typeof(RequestType))
                                 .Cast<RequestType>()
                                 .Select(enumValue => new LookUpDataModel<int>
                                 {
                                     Value = Convert.ToInt32(enumValue),
                                     NameAr = enumValue.ToString(),
                                     NameEn = enumValue.ToString()
                                 }).ToList();
                return new ResponseModel<List<LookUpDataModel<int>>> { Result = result, IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<LookUpDataModel<int>>>
                {
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }
        
        public async Task<ResponseModel<List<LookUpDataModel<int>>>> ProductServiceRequestStatusLookup()
        {
            try
            {
                var result = Enum.GetValues(typeof(ServiceRequestStatus))
                                 .Cast<ServiceRequestStatus>()
                                 .Select(enumValue => new LookUpDataModel<int>
                                 {
                                     Value = Convert.ToInt32(enumValue),
                                     NameAr = enumValue.ToString(),
                                     NameEn = enumValue.ToString()
                                 }).ToList();
                return new ResponseModel<List<LookUpDataModel<int>>> { Result = result, IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<LookUpDataModel<int>>>
                {
                    IsSuccess = false,
                    Message = "ErrorFound"
                };
            }
        }

        public async Task<ResponseModel<bool>> ResponseToRequest(int RequestId, UpdateRequestDto updateRequestDto)
        {
            try { 
            var service = await _unitOfWork.ServiceRequests.GetByIdAsync(RequestId);
            if (service == null)
            {
                return new ResponseModel<bool> { IsSuccess = false, Message = "ServiceNotFound" };
            }
            service.ServiceRequestStatus = updateRequestDto.ServiceRequestStatus;
            service.ResponseDate = DateTime.Now;
            service.ResponseDetails = updateRequestDto.ResponseDetails;
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel<bool> {IsSuccess = true};
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool> { IsSuccess = false, Message = "ErrorFound" };
            }
        }

    }
}
