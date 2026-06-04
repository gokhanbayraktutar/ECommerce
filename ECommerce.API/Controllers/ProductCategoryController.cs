using ECommerce.Application.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductCategoryController : ControllerBase
{
    private readonly IProductCategoryService _service;

    public ProductCategoryController(IProductCategoryService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{productId}/{categoryId}")]
    public async Task<IActionResult> Get(int productId, int categoryId)
    {
        var entity = await _service.GetByIdAsync(productId, categoryId);
        return entity == null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCategoryDto dto)
    {
        var entity = new ProductCategory { ProductId = dto.ProductId, CategoryId = dto.CategoryId };
        await _service.AddAsync(entity);
        return Ok(entity);
    }

    [HttpPut("{productId}/{categoryId}")]
    public async Task<IActionResult> Update(int productId, int categoryId, ProductCategoryDto dto)
    {
        var entity = await _service.GetByIdAsync(productId, categoryId);
        if (entity == null) return NotFound();

        entity.ProductId = dto.ProductId;
        entity.CategoryId = dto.CategoryId;

        await _service.UpdateAsync(entity);
        return Ok(entity);
    }

    [HttpDelete("{productId}/{categoryId}")]
    public async Task<IActionResult> Delete(int productId, int categoryId)
    {
        await _service.DeleteAsync(productId, categoryId);
        return Ok(new { message = "Silindi" });
    }
}