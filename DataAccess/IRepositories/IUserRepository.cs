
using System.Linq.Expressions;
using Common;
using DataAccess.IRepositories;
using DataAccess.Models;

namespace DataAccess.IRepositories
{
    public interface IUserRepository : IRepository<AppUser>
    {
        Task<IEnumerable<AppUser>> GetAllUsers(Expression<Func<AppUser, bool>> filter = null);
        Task<AppUser> GetUserById(string id);
        Task CreateUser(AppUser user);
        Task UpdateUser(AppUser user);
        Task<AppUser> GetUserByEmail(string email);
        Task<AppUser> GetUserByName(string Name);
        Task<IEnumerable<AppUser>> GetClientsWithTicketsAsync();
        Task<AppUser> GetUserByMobileNumber(string email);
        Task<AppUser> getUserById(string id);
        Task<List<AppUser>> getUsersByType(UserType type);
    }
}
