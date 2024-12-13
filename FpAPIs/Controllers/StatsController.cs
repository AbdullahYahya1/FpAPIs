using Common;
using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GoodApiHereForYou.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [CustomAuthorize([UserType.Manager])]
    public class StatsController : ControllerBase
    {
        private readonly FPDbContext _context;
        public StatsController(FPDbContext context)
        {
            _context = context;
        }

        [HttpGet("SalesOverTime")]
        public async Task<IActionResult> GetSalesOverTime()
        {
            var salesOverTime = await _context.Orders
                .Where(o => o.Transaction!=null)
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new SalesOverTimeDTO
                {
                    Date = g.Key,
                    TotalSales = g.Sum(o => o.TotalPrice)
                })
                .OrderBy(o => o.Date)
                .ToListAsync();

            return Ok(new ResponseModel<List<SalesOverTimeDTO>>
            {
                Result = salesOverTime,
                IsSuccess = true,
                Message = null
            });
        }

        [HttpGet("OrdersByStatus")]
        public async Task<IActionResult> GetOrdersByStatus()
        {
            var ordersByStatus = await _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new OrdersByStatusDTO
                {
                    Status = g.Key.ToString(),
                    TotalOrders = g.Count()
                })
                .ToListAsync();

            return Ok(new ResponseModel<List<OrdersByStatusDTO>>
            {
                Result = ordersByStatus,
                IsSuccess = true,
                Message = null
            });
        }

        [HttpGet("TopCustomers")]
        public async Task<IActionResult> GetTopCustomers()
        {
            var topCustomers = await _context.Orders
                .GroupBy(o => o.CustomerId)
                .Select(g => new TopCustomersDTO
                {
                    CustomerName = g.FirstOrDefault().Customer.UserName,
                    CustomerPhone = g.FirstOrDefault().Customer.MobileNumber,
                    TotalOrders = g.Count()
                })
                .OrderByDescending(c => c.TotalOrders)
                .Take(10)
                .ToListAsync();

            return Ok(new ResponseModel<List<TopCustomersDTO>>
            {
                Result = topCustomers,
                IsSuccess = true,
                Message = null
            });
        }

        [HttpGet("SalesByCategory")]
        public async Task<IActionResult> GetSalesByCategory()
        {
            var salesByCategory = await _context.OrderItems
                .Include(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .GroupBy(oi => oi.Product.Category.NameEn)
                .Select(group => new SalesByCategoryDTO
                {
                    Category = group.Key,
                    TotalSales = group.Sum(oi => oi.Quantity * oi.Product.Price)
                })
                .ToListAsync();

            return Ok(new ResponseModel<List<SalesByCategoryDTO>>
            {
                Result = salesByCategory,
                IsSuccess = true,
                Message = null
            });
        }

        [HttpGet("NewUsersOverTime")]
        public async Task<IActionResult> GetNewUsersOverTime()
        {
            var newUsers = await _context.Users
                .GroupBy(u => u.DateCreated.Date)
                .Select(group => new NewUsersOverTimeDTO
                {
                    Date = group.Key,
                    NewUserCount = group.Count()
                })
                .ToListAsync();

            return Ok(new ResponseModel<List<NewUsersOverTimeDTO>>
            {
                Result = newUsers,
                IsSuccess = true,
                Message = null
            });
        }

        [HttpGet("OrdersOverTime")]
        public async Task<IActionResult> GetOrdersOverTime()
        {
            var ordersOverTime = await _context.Orders
                .GroupBy(o => o.CreatedAt.Date)
                .Select(group => new OrdersOverTimeDTO
                {
                    Date = group.Key,
                    OrderCount = group.Count()
                })
                .ToListAsync();

            return Ok(new ResponseModel<List<OrdersOverTimeDTO>>
            {
                Result = ordersOverTime,
                IsSuccess = true,
                Message = null
            });
        }

        [HttpGet("ShippingStatus")]
        public async Task<IActionResult> GetShippingStatus()
        {
            var shippingStatus = await _context.Orders
                .GroupBy(o => o.ShippingStatus)
                .Select(group => new ShippingStatusDTO
                {
                    ShippingStatus = group.Key.ToString(),
                    Count = group.Count()
                })
                .ToListAsync();

            return Ok(new ResponseModel<List<ShippingStatusDTO>>
            {
                Result = shippingStatus,
                IsSuccess = true,
                Message = null
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAdminDashboardStats()
        {
            var newOrdersCount = await _context.Orders
                .Where(o => o.CreatedAt.Date == DateTime.UtcNow.Date)
                .CountAsync();

            var newUsersCount = await _context.Users
                .Where(u => u.DateCreated.Date == DateTime.UtcNow.Date)
                .CountAsync();

            var totalOrders = await _context.Orders.CountAsync();

            var totalSales = await _context.Orders
                .Where(o => o.Transaction !=null)
                .SumAsync(o => (decimal?)o.TotalPrice) ?? 0;

            var pendingServiceRequests = await _context.ServiceRequests
                .Where(sr => sr.ServiceRequestStatus == ServiceRequestStatus.New)
                .CountAsync();

            var completedOrdersToday = await _context.Orders
                .Where(o => o.Status == OrderStatus.Complete && o.CreatedAt.Date == DateTime.UtcNow.Date)
                .CountAsync();

            var productsInStock = await _context.Products
                .Where(p => p.ProductStatus == ProductStatus.Active)
                .CountAsync();

            var totalClients = await _context.Users.Where(u => u.UserType == UserType.Client).CountAsync();

            var cancelledOrders = await _context.Orders
                .Where(o => o.Status == OrderStatus.Cancelled)
                .CountAsync();

            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var totalRevenueThisMonth = await _context.Orders
            .Where(o => o.Transaction != null && o.CreatedAt >= thirtyDaysAgo)
            .SumAsync(o => (decimal?)o.TotalPrice) ?? 0;

            var stats = new
            {
                NewOrdersCountToday = newOrdersCount,
                NewUsersCountToday = newUsersCount,
                TotalOrders = totalOrders,
                TotalSales = totalSales,
                PendingServiceRequests = pendingServiceRequests,
                CompletedOrdersToday = completedOrdersToday,
                ProductsInStock = productsInStock,
                TotalClients = totalClients,
                CancelledOrders = cancelledOrders,
                TotalRevenueThisMonth = totalRevenueThisMonth,
            };

            return Ok(new ResponseModel<object>
            {
                Result = stats,
                IsSuccess = true,
                Message = null
            });
        }
    }
}
 