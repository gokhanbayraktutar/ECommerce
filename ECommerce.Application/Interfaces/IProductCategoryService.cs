using ECommerce.Core.Entities;

namespace ECommerce.Application.Interfaces;

public interface IProductCategoryService
{
    Task<IEnumerable<ProductCategory>> GetAllAsync();
    Task<ProductCategory?> GetByIdAsync(int productId, int categoryId);
    Task AddAsync(ProductCategory productCategory);
    Task UpdateAsync(ProductCategory productCategory);
    Task DeleteAsync(int productId, int categoryId);
}