using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.IRepositories;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        private readonly WADbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(WADbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers(Expression<Func<AppUser, bool>> filter = null)
        {
            if (filter != null)
            {
                return await _context.Users.Where(filter).ToListAsync();
            }

            return await _context.Users.ToListAsync();
        }


        public async Task<AppUser?> GetUserById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task CreateUser(AppUser user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(AppUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<AppUser> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AppUser> GetUserByName(string Name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == Name);
        }


        public async Task<IEnumerable<AppUser>> GetClientsWithTicketsAsync()
        {
            return await _context.Users
                .Where(u => u.UserType == UserType.Client).ToListAsync();
        }

    }
}
