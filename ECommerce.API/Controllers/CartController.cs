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
        return Ok(cart);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddItem(int productId, int quantity)
    {
        var cart = await _cartService.AddItemAsync(GetUserId(), productId, quantity);

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
}
