using ECommerce.Application.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;

namespace ECommerce.Application.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;

    public CartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Cart>> GetAllAsync()
    {
        var carts = (await _unitOfWork.Carts
            .FindAsync(c => c.OrderStatus != "Sepette"))
            .ToList();

        foreach (var cart in carts)
        {
            cart.CartItems = (await _unitOfWork.CartItems
                .FindAsync(ci => ci.CartId == cart.Id))
                .ToList();

            foreach (var item in cart.CartItems)
            {
                item.Product = await _unitOfWork.Products
                    .GetByIdAsync(item.ProductId);
            }
        }

        return carts
             .OrderBy(x => x.OrderStatus == "Sipariş Alındı" ? 0 : 1)
             .ThenByDescending(x => x.OrderStatus == "Sipariş Alındı" ? x.OrderDate : DateTime.MinValue);

    }


    public async Task<Cart> GetCartByUserIdAsync(int userId)
{
        var carts = await _unitOfWork.Carts.FindAsync(c => c.UserId == userId && c.OrderStatus == "Sepette");
        var cart = carts.FirstOrDefault();

    if (cart != null)
    {
        // CartItems ve ürünleri tek tek yükle
        cart.CartItems = (await _unitOfWork.CartItems.FindAsync(ci => ci.CartId == cart.Id)).ToList();

        foreach (var item in cart.CartItems)
        {
            item.Product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
        }
    }

    return cart;
}


    public async Task<Cart> AddItemAsync(int userId, int productId, int quantity, decimal price)
    {
        var cart = await GetCartByUserIdAsync(userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                OrderStatus = "Sepette",
                CartItems = new List<CartItem>()
            };

            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.CommitAsync();
        }

        var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            existingItem.TotalPrice = existingItem.Price * existingItem.Quantity;  // 🔥 eklendi
        }
        else
        {
            var newItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity,
                Price = price,
                TotalPrice = price * quantity
            };

            await _unitOfWork.CartItems.AddAsync(newItem);
        }

        await _unitOfWork.CommitAsync();

        cart.CartItems = (await _unitOfWork.CartItems.FindAsync(ci => ci.CartId == cart.Id)).ToList();

        foreach (var item in cart.CartItems)
        {
            item.Product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
        }

        cart.TotalPaymentPrice = cart.CartItems.Sum(ci => ci.TotalPrice);

        _unitOfWork.Carts.Update(cart);
        await _unitOfWork.CommitAsync();

        return cart;
    }


    public async Task RemoveItemAsync(int userId, int cartItemId)
    {
        var cart = await GetCartByUserIdAsync(userId);
        var item = cart?.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

        if (item != null)
        {
            _unitOfWork.CartItems.Delete(item);
            await _unitOfWork.CommitAsync();
        }
    }

    public async Task UpdateItemQuantityAsync(int userId, int cartItemId, int quantity)
    {
        var cart = await GetCartByUserIdAsync(userId);
        var item = cart?.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

        if (item != null)
        {
            item.Quantity = quantity;
            _unitOfWork.CartItems.Update(item);
            await _unitOfWork.CommitAsync();
        }
    }

    public async Task<OrderDetailDto> GetOrderDetailAsync(int userId, int cartId)
    {
        var carts = await _unitOfWork.Carts.FindAsync(c =>
            c.Id == cartId &&
            c.UserId == userId 
        );

        var cart = carts.FirstOrDefault();
        if (cart == null)
            return null;

        cart.CartItems = (await _unitOfWork.CartItems.FindAsync(ci =>
            ci.CartId == cart.Id
        )).ToList();

        foreach (var item in cart.CartItems)
        {
            item.Product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
        }

        return new OrderDetailDto
        {
            CartId = cart.Id,
            OrderNo = cart.OrderNo,
            OrderDate = cart.OrderDate,
            TotalPrice = cart.TotalPaymentPrice ?? 0,
            PaymentType = cart.PaymentType,
            FullName = cart.FullName,
            Phone = cart.Phone,
            Address = cart.Address,

            Items = cart.CartItems.Select(ci => new OrderItemDto
            {
                Id = ci.Id,
                ProductName = ci.Product?.Name,
                Picture = ci.Product?.Picture,
                Price = ci.Price ?? 0,
                Quantity = ci.Quantity,
                TotalPrice = ci.TotalPrice ?? 0
            }).ToList()
        };
    }

    public async Task<List<OrderSummaryDto>> GetOrdersByUserIdAsync(int userId)
    {
        var carts = await _unitOfWork.Carts.FindAsync(c =>
            c.UserId == userId &&
            c.OrderStatus != "Sepette"
        );

        return carts.Select(cart => new OrderSummaryDto
        {
            CartId = cart.Id,
            OrderNo = cart.OrderNo,
            OrderDate = cart.OrderDate,
            TotalPrice = cart.TotalPaymentPrice ?? 0,
            OrderStatus = cart.OrderStatus,
            PaymentType = cart.PaymentType
        }).ToList();
    }

    public async Task UpdateOrderStatusAsync(int cartId, string status)
    {
        var carts = await _unitOfWork.Carts.FindAsync(c => c.Id == cartId);
        var cart = carts.FirstOrDefault();

        if (cart == null)
            throw new Exception("Sipariş bulunamadı");

        cart.OrderStatus = status;

        _unitOfWork.Carts.Update(cart);
        await _unitOfWork.CommitAsync();
    }


}
