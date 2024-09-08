using DataAccess.DTOs;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IServiceRequestRepository:IRepository<ServiceRequest>
    {
        Task<List<ServiceRequest>> GetAllWithImgsAsync();
        Task<List<ServiceRequest>> GetAllWithImgsAsyncByUserId(string UserId);
        Task<ServiceRequest> GetServiceWithImgsByServiceId(int ServiceID);

    }
}
