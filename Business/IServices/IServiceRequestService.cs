using DataAccess.DTOs;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IServiceRequestService :IService<ServiceRequest>
    {
        Task<ResponseModel<bool>> CreateService(PostServiceDto serviceRequest);
        Task<ResponseModel<List<GetServiceDto>>> GetServices();
        
        Task<ResponseModel<GetServiceDto>> GetServicesByID(int ServiceId);
        Task<ResponseModel<List<LookUpDataModel<int>>>> ProductServiceRequestStatusLookup();
        Task<ResponseModel<List<LookUpDataModel<int>>>> ProductRequestTypeLookup();
    }
}
