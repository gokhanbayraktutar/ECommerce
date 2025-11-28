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
    public async Task<IActionResult> GetCart() => Ok(await _cartService.GetCartByUserIdAsync(GetUserId()));

    [HttpPost("add")]
    public async Task<IActionResult> AddItem(int productId, int quantity)
    {
        await _cartService.AddItemAsync(GetUserId(), productId, quantity);
        return Ok();
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateItem(int cartItemId, int quantity)
    {
        await _cartService.UpdateItemQuantityAsync(GetUserId(), cartItemId, quantity);
        return Ok();
    }

    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveItem(int cartItemId)
    {
        await _cartService.RemoveItemAsync(GetUserId(), cartItemId);
        return Ok();
    }
}
