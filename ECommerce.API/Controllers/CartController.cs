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
        await _cartService.AddItemAsync(GetUserId(), productId, quantity);
        return Ok("Sepete Ekleme Başarılı!");
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
