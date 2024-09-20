using AutoMapper;
using Business.IServices;
using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.IRepositories;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class OrderService : Service<Order>, IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(FPDbContext context, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel<GetOrderDto>> AddOrder(PostOrderDto postOrder)
        {
            // Get current user information from HttpContext
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ResponseModel<GetOrderDto>() { IsSuccess = false, Message = "User is not authenticated." };
            }

            // Retrieve the shipping address and validate ownership
            var address = await _unitOfWork.UserAddresses.GetByIdAsync(postOrder.ShippingAddressId);
            if (address == null || address.UserId != currentUserId)
            {
                return new ResponseModel<GetOrderDto>() { IsSuccess = false, Message = "Invalid or unauthorized shipping address." };
            }

            // Get the cart items for the current user
            var carItems = await _unitOfWork.CartItems.GetCartItemProductsByUserID(currentUserId);
            if (carItems == null || !carItems.Any())
            {
                return new ResponseModel<GetOrderDto>() { Message = "No items in cart.", IsSuccess = false };
            }

            // Validate if any product is already inactive
            var invalidProduct = carItems.FirstOrDefault(item => item.Product.ProductStatus != ProductStatus.Active);
            if (invalidProduct != null)
            {
                return new ResponseModel<GetOrderDto>() { IsSuccess = false, Message = "One or more items in your cart are no longer available." };
            }

            // Begin transaction to ensure atomicity
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var order = new Order()
                    {
                        ShippingAddressId = postOrder.ShippingAddressId,
                        CustomerId = currentUserId,
                        ShippingStatus = ShippingStatus.NotShipped,
                        Status = OrderStatus.Pending,
                        CreatedAt = DateTime.UtcNow,
                        TotalPrice = 0,
                        OrderItems = new List<OrderItem>()
                    };

                    await _unitOfWork.Orders.AddAsync(order);
                    await _unitOfWork.SaveChangesAsync();
                    List<int> productIdsInOrder = new List<int>();

                    // Process each cart item, deactivate product, and add order items
                    foreach (var item in carItems)
                    {
                        // Deactivate the product and update the status
                        item.Product.ProductStatus = ProductStatus.Inactive;
                        await _unitOfWork.Products.UpdateAsync(item.Product);

                        // Add order items
                        var orderItem = new OrderItem() { OrderId = order.OrderId, ProductId = item.ProductId, Quantity = 1 };
                        order.OrderItems.Add(orderItem);

                        // Update order total price
                        order.TotalPrice += item.Product.Price;

                        // Add the product ID to the list for later use
                        productIdsInOrder.Add(item.ProductId);
                    }

                    // Update the order with calculated total price
                    await _unitOfWork.Orders.UpdateAsync(order);
                    await _unitOfWork.SaveChangesAsync();

                    // Remove all cart items that contain any of the products in this order
                    // 1. Remove from other users' carts
                    await _unitOfWork.CartItems.RemoveCartItemsByProductIds(productIdsInOrder);
                    // 2. Remove from current user's cart
                    await _unitOfWork.CartItems.RemoveCartItemsForUserByProductIds(currentUserId, productIdsInOrder);

                    // Save changes for cart item deletions
                    await _unitOfWork.SaveChangesAsync();
                    // Commit the transaction to finalize the order
                    transaction.Commit();
                    // Map the order to the DTO for returning
                    var orderDto = _mapper.Map<GetOrderDto>(order);
                    return new ResponseModel<GetOrderDto>()
                    {
                        Result = orderDto,
                        IsSuccess = true
                    };
                }
                catch (Exception ex)
                {
                    if (transaction != null && transaction.GetDbTransaction()?.Connection != null)
                    {
                        transaction.Rollback();
                    }

                    return new ResponseModel<GetOrderDto>()
                    {
                        IsSuccess = false,
                        Message = $"Failed to create the order. Error: {ex.Message}. StackTrace: {ex.StackTrace}"
                    };
                }
            }
        }

        public async Task<ResponseModel<List<GetOrderDto>>> GetOrders()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            var orders =await _unitOfWork.Orders.GetOrdersAsync(currentUserId);
            var ordersDto = _mapper.Map<List<GetOrderDto>>(orders);
            return new ResponseModel<List<GetOrderDto>> { Result = ordersDto, IsSuccess = true }; 
        }

        public async Task<ResponseModel<GetOrderDto>> PayOrder(int OrderId, PayOrderDto payOrderDto)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            var orders =await _unitOfWork.Orders.GetOrdersAsync(currentUserId);
            var order = orders.FirstOrDefault(O => O.OrderId == OrderId);
            if(order == null)
            {
                return new ResponseModel<GetOrderDto>
                {
                    IsSuccess = false,
                    Message = "No Order Found"
                };
            }
            var transatction = new UserPurchaseTransaction()
            {
                CardholderName = payOrderDto.CardholderName,
                CreatedById = currentUserId,
                TotalPrice = order.TotalPrice,
                TransactionDate = DateTime.UtcNow,
                Provider= payOrderDto.Cash? PaymentProvider.Cash  : PaymentProvider.Visa ,
                TransactionStatus=TransactionStatus.Payed
            };
            await _unitOfWork.userPurchaseTransactions.AddAsync(transatction);
            await _unitOfWork.SaveChangesAsync();
            order.TransactionId = transatction.TransactionId;
            order.Status = OrderStatus.Processing;
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();   
            var orderDto = _mapper.Map<GetOrderDto>(order);
            return new ResponseModel<GetOrderDto> { IsSuccess = true, Result = orderDto };
        }
    }
}
