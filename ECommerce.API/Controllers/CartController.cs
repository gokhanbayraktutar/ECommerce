using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var cart = await _cartService.GetCartByUserIdAsync(GetUserId());

        return Ok(new
        {
            cartItems = cart?.CartItems?.Select(ci => new
            {
                id = ci.Id,
                productId = ci.ProductId,
                quantity = ci.Quantity,
                price = ci.Price,
                totalPrice = ci.TotalPrice,
                product = new
                {
                    id = ci.Product.Id,
                    name = ci.Product.Name,
                    price = ci.Product.Price,
                    picture = ci.Product.Picture
                }
            }) ?? Enumerable.Empty<object>(),

            totalPaymentPrice = cart?.TotalPaymentPrice ?? 0
        });
    }


    [HttpPost("add")]
    public async Task<IActionResult> AddItem(int productId, int quantity, decimal price)
    {
        var cart = await _cartService.AddItemAsync(GetUserId(), productId, quantity,price);

        var result = new
        {
            cartItems = cart.CartItems.Select(ci => new
            {
                id = ci.Id,
                productId = ci.ProductId,
                quantity = ci.Quantity,
                product = new
                {
                    id = ci.Product.Id,
                    name = ci.Product.Name,
                    price = ci.Product.Price,
                    picture = ci.Product.Picture
                }
            }).ToList()
        };

        return Ok(result);
    }


    [HttpDelete("remove/{cartItemId}")]
    public async Task<IActionResult> RemoveItem(int cartItemId)
    {
        await _cartService.RemoveItemAsync(GetUserId(), cartItemId);
        return Ok();
    }

    [HttpPut("update/{cartItemId}")]
    public async Task<IActionResult> UpdateItem(int cartItemId, int quantity)
    {
        await _cartService.UpdateItemQuantityAsync(GetUserId(), cartItemId, quantity);
        return Ok();
    }

    [HttpGet("order/{cartId}")]
    public async Task<IActionResult> GetOrderDetail(int cartId)
    {
        var result = await _cartService.GetOrderDetailAsync(GetUserId(), cartId);

        if (result == null)
            return NotFound("Sipariş bulunamadı");

        return Ok(result);
    }

    [HttpGet("myorders")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = GetUserId();
        var orders = await _cartService.GetOrdersByUserIdAsync(userId);

        return Ok(orders);
    }
}
