using ECommerce.Core.Entities;

namespace ECommerce.Application.Interfaces;

public interface ICartService
{
    Task<Cart> GetCartByUserIdAsync(int userId);
    Task AddItemAsync(int userId, int productId, int quantity);
    Task RemoveItemAsync(int userId, int cartItemId);
    Task UpdateItemQuantityAsync(int userId, int cartItemId, int quantity);
}
