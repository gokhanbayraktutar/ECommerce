using ECommerce.Application.DTO;
using ECommerce.Core.Entities;
using Refit;

public interface IProductCategoryApiClient
{
    [Get("/productcategory")]
    Task<List<ProductCategory>> GetAllAsync();

    [Get("/productcategory/{productId}/{categoryId}")]
    Task<ProductCategory> GetByIdAsync(int productId, int categoryId);

    [Post("/productcategory")]
    Task<ApiResponse<ProductCategory>> CreateAsync([Body] ProductCategoryDto productCategory);

    [Put("/productcategory/{productId}/{categoryId}")]
    Task<ApiResponse<string>> UpdateAsync(int productId, int categoryId, [Body] ProductCategoryDto productCategory);

    [Delete("/productcategory/{productId}/{categoryId}")]
    Task<HttpResponseMessage> DeleteAsync(int productId, int categoryId);
}