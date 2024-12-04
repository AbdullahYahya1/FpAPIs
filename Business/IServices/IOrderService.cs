using DataAccess.DTOs;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IOrderService:IService<Order>
    {
        Task<ResponseModel<GetOrderDto>> AddOrder(PostOrderDto postOrder);
        Task<ResponseModel<List<GetOrderDto>>> GetOrders();
        Task<ResponseModel<GetOrderDto>> PayOrder(int OrderId, PayOrderDto payOrderDto);
        Task<ResponseModel<bool>> AssignDriver(int OrderId, string UserId);
        Task<ResponseModel<bool>> DeliverOrder(int OrderId);
        Task<ResponseModel<bool>> CancelOrder(int OrderId);
        Task<ResponseModel<bool>> PickUpOrder(int OrderId);

    }
}
