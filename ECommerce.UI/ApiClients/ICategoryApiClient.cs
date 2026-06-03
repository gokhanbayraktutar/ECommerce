using Refit;
using ECommerce.Core.Entities;

namespace ECommerce.UI.ApiClients;

public interface ICategoryApiClient
{
    [Get("/categories")]
    Task<List<Category>> GetAllAsync();

    [Get("/categories/main")]
    Task<List<Category>> GetMainCategoriesAsync();

    [Get("/categories/{id}")]
    Task<Category> GetByIdAsync(int id);

    [Post("/categories")]
    Task<ApiResponse<Category>> CreateAsync([Body] Category category);

    [Put("/categories/{id}")]
    Task<ApiResponse<string>> UpdateAsync(int id, [Body] Category category);

    [Delete("/categories/{id}")]
    Task<HttpResponseMessage> DeleteAsync(int id);
}