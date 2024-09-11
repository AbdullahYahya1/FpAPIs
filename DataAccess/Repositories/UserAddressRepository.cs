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
    public class UserAddressRepository : Repository<UserAddress>, IUserAddressRepository
    {
        private readonly FPDbContext _context;
        public UserAddressRepository(FPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<UserAddress>> GetUserAddressesAsync(string UserId)
        {
            var addresses= await _context.UserAddresses.Where(A=>A.UserId== UserId).ToListAsync();
            return addresses; 
        }
    }
}
