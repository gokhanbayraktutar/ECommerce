using ECommerce.Core.Entities;
using ECommerce.UI.ApiClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Refit;
using System.Text.Json;

namespace ECommerce.UI.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly ICategoryApiClient _categoryApiClient;

    public CategoryController(ICategoryApiClient categoryApiClient)
    {
        _categoryApiClient = categoryApiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryApiClient.GetAllAsync();

        var sortedCategories = categories
        .OrderBy(c => c.ParentCategoryId ?? c.Id)
        .ThenBy(c => c.ParentCategoryId == null ? 0 : 1)
        .ToList();

        return View(sortedCategories);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateMainCategoriesDropdown();
        return View(new Category());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        var response = await _categoryApiClient.CreateAsync(category);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }

        await HandleApiErrorAsync(response);
        await PopulateMainCategoriesDropdown();
        return View(category);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var category = await _categoryApiClient.GetByIdAsync(id);
            if (category == null) return NotFound();

            await PopulateMainCategoriesDropdown();
            return View(category);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category category)
    {
        if (id != category.Id) return BadRequest();

        var response = await _categoryApiClient.UpdateAsync(id, category);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }

        await HandleApiErrorAsync(response);
        await PopulateMainCategoriesDropdown();
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] 
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _categoryApiClient.DeleteAsync(id);

        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] = "Kategori silinirken bir hata oluştu. Alt kategorileri olabilir.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateMainCategoriesDropdown()
    {
        var mainCategories = await _categoryApiClient.GetMainCategoriesAsync();
        ViewBag.MainCategories = new SelectList(mainCategories, "Id", "Name");
    }

    private async Task HandleApiErrorAsync<T>(ApiResponse<T> response)
    {
        try
        {
            if (!string.IsNullOrEmpty(response.Error?.Content))
            {
                using var doc = JsonDocument.Parse(response.Error.Content);
                if (doc.RootElement.TryGetProperty("message", out var msg))
                {
                    ModelState.AddModelError("", msg.GetString() ?? "Bir hata oluştu.");
                    return;
                }
            }
            ModelState.AddModelError("", "İşlem gerçekleştirilemedi.");
        }
        catch
        {
            ModelState.AddModelError("", "API sunucusuyla iletişim kurulurken bir hata oluştu.");
        }
    }
}