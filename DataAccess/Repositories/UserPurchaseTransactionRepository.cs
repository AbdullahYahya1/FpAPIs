using DataAccess.Context;
using DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserPurchaseTransactionRepository : Repository<UserPurchaseTransaction>, IUserPurchaseTransactionRepository
    {
        public UserPurchaseTransactionRepository(FPDbContext context) : base(context)
        {
        }
    }
}
