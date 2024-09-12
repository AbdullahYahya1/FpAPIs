using Business.IServices;
using DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UserPurchaseTransactionService : Service<UserPurchaseTransaction>, IUserPurchaseTransactionService
    {
        public UserPurchaseTransactionService(FPDbContext context) : base(context)
        {
        }
    }
}
