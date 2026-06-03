using ECommerce.Core.Entities;

namespace ECommerce.Application.Interfaces;

public interface ICategoryService
{
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetMainCategoriesAsync();
    Task<IEnumerable<Category>> GetSubCategoriesByParentIdAsync(int parentId);
}