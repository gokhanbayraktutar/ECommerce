using ECommerce.Application.DTO;
using ECommerce.UI.ApiClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Refit;
using System.Text.Json;

namespace ECommerce.UI.Areas.Admin.Controllers;

[Area("Admin")]

public class ProductCategoryController : Controller
{
    private readonly IProductCategoryApiClient _apiClient;
    private readonly ICategoryApiClient _categoryApiClient;
    private readonly IProductApiClient _productApiClient;

    public ProductCategoryController(IProductCategoryApiClient apiClient, ICategoryApiClient categoryApiClient, IProductApiClient productApiClient)
    {
        _apiClient = apiClient;
        _categoryApiClient = categoryApiClient;
        _productApiClient = productApiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var list = await _apiClient.GetAllAsync();

        await PopulateDropdownsAsync();

        return View(list);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateDropdownsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCategoryDto model)
    {
        var response = await _apiClient.CreateAsync(model);
        if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

        await PopulateDropdownsAsync();
        await HandleApiErrorAsync(response);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int productId, int categoryId)
    {
        var response = await _apiClient.DeleteAsync(productId, categoryId);
        if (!response.IsSuccessStatusCode) TempData["ErrorMessage"] = "Silme işlemi başarısız.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdownsAsync()
    {
        //ViewBag.Products = new SelectList(await _productApiClient.GetAllAsync(), "Id", "Name");
        ViewBag.Categories = new SelectList(await _categoryApiClient.GetAllAsync(), "Id", "Name");
    }

    private async Task HandleApiErrorAsync<T>(ApiResponse<T> response)
    {
        var errorContent = response.Error?.Content;
        if (!string.IsNullOrEmpty(errorContent))
        {
            using var doc = JsonDocument.Parse(errorContent);
            if (doc.RootElement.TryGetProperty("message", out var msg))
                ModelState.AddModelError("", msg.GetString() ?? "Hata oluştu.");
        }
        else
        {
            ModelState.AddModelError("", "İşlem yapılamadı.");
        }
    }
}