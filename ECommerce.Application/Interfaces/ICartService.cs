using ECommerce.Application.DTO;
using ECommerce.Core.Entities;

namespace ECommerce.Application.Interfaces;

public interface ICartService
{
    Task<IEnumerable<Cart>> GetAllAsync();
    Task<Cart> GetCartByUserIdAsync(int userId);
    Task <Cart> AddItemAsync(int userId, int productId, int quantity, decimal price);
    Task RemoveItemAsync(int userId, int cartItemId);
    Task UpdateItemQuantityAsync(int userId, int cartItemId, int quantity);

    Task<OrderDetailDto> GetOrderDetailAsync(int userId, int cartId);

    Task<List<OrderSummaryDto>> GetOrdersByUserIdAsync(int userId);
    Task UpdateOrderStatusAsync(int cartId, string status);

}
