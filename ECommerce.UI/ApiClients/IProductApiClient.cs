using Refit;
using ECommerce.Core.Entities;

namespace ECommerce.UI.ApiClients;

public interface IProductApiClient
{
    [Get("/products")]
    Task<List<Product>> GetAllAsync();

    [Get("/products/{id}")]
    Task<Product> GetByIdAsync(int id);

    [Post("/products")]
    Task<ApiResponse<Product>> CreateAsync([Body] Product product);

    [Put("/products/{id}")]
    Task<ApiResponse<string>> UpdateAsync(int id, [Body] Product product);

    [Delete("/products/{id}")]
    Task<HttpResponseMessage> DeleteAsync(int id);
}