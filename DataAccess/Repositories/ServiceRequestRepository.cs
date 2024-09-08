using DataAccess.Context;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ServiceRequestRepository : Repository<ServiceRequest>, IServiceRequestRepository
    {
        private readonly FPDbContext _context;
        public ServiceRequestRepository(FPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ServiceRequest>> GetAllWithImgsAsync()
        {
            var serviceRequsts = await _context.ServiceRequests.Include(S => S.Images).ToListAsync();
            return serviceRequsts;
        }

        public async Task<List<ServiceRequest>> GetAllWithImgsAsyncByUserId(string UserId)
        {
            var serviceRequsts = await _context.ServiceRequests.Include(S => S.Images).Where(S=>S.CreatedById==UserId).ToListAsync();
            return serviceRequsts;
        }

        public async Task<ServiceRequest> GetServiceWithImgsByServiceId(int ServiceID)
        {
            var serviceRequsts = await _context.ServiceRequests.Include(S => S.Images).FirstOrDefaultAsync(S => S.RequestId == ServiceID);
            return serviceRequsts;
        }
    }
}
