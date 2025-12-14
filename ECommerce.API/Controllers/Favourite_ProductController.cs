using ECommerce.Application.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class Favourite_ProductController : ControllerBase
{
    private readonly IFavourite_ProductService _favouriteService;

    public Favourite_ProductController(IFavourite_ProductService favouriteService)
    {
        _favouriteService = favouriteService;
    }

 
    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        var favourites = await _favouriteService.GetByUserIdAsync(userId);
        return Ok(favourites);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Favourite_Product favourite)
    {
        favourite.UserId = GetUserId();
        favourite.AddedDate = DateTime.UtcNow;

        await _favouriteService.AddAsync(favourite);
        return Ok(favourite);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _favouriteService.DeleteAsync(id);
        return NoContent();
    }
}
