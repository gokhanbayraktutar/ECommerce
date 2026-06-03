using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("main")]
    public async Task<IActionResult> GetMainCategories()
    {
        var mainCategories = await _categoryService.GetMainCategoriesAsync();
        return Ok(mainCategories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpGet("{parentId}/subcategories")]
    public async Task<IActionResult> GetSubCategories(int parentId)
    {
        var subCategories = await _categoryService.GetSubCategoriesByParentIdAsync(parentId);
        return Ok(subCategories);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        try
        {
            await _categoryService.AddAsync(category);
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Category category)
    {
        if (id != category.Id) return BadRequest();

        try
        {
            await _categoryService.UpdateAsync(category);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteAsync(id);
        return NoContent();
    }
}