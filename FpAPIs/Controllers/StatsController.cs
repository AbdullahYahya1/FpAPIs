using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GoodApiHereForYou.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly FPDbContext _context;
        public StatsController(FPDbContext context)
        {
            _context = context;
        }

        // SalesOverTime, OrdersByStatus, TopCustomers, SalesByCategory, TransactionSuccessRate, NewUsersOverTime, OrdersOverTime, ShippingStatus

        // 1. Total Sales Over Time (Line Chart)
        [HttpGet("SalesOverTime")]
        public async Task<IActionResult> GetSalesOverTime()
        {
            var salesOverTime = await _context.Orders
                .Where(o => o.Status == OrderStatus.Complete)
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

        // 2. Orders By Status (Pie or Bar Chart)
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

        // 3. Top Customers By Orders (Bar Chart)
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

        // 4. Sales By Category (Bar Chart)
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

        // 6. New Users Over Time (Line Chart)
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

        // 7. Orders Over Time (Line Chart)
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

        // 8. Shipping Status (Bar Chart)
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
    }
}
 