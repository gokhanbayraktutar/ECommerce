using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;

namespace ECommerce.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddAsync(Category category)
    {
        if (category.ParentCategoryId.HasValue)
        {
            var parentExists = await _unitOfWork.Categories.GetByIdAsync(category.ParentCategoryId.Value);
            if (parentExists == null)
            {
                throw new Exception("Belirtilen üst kategori sistemde bulunamadı!");
            }
        }

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        if (category.ParentCategoryId.HasValue && category.ParentCategoryId.Value == category.Id)
        {
            throw new Exception("Bir kategori kendisinin üst kategorisi olarak seçilemez!");
        }

        _unitOfWork.Categories.Update(category);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category != null)
        {
            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.CommitAsync();
        }
    }

    public async Task<IEnumerable<Category>> GetAllAsync() => await _unitOfWork.Categories.GetAllAsync();

    public async Task<Category> GetByIdAsync(int id) => await _unitOfWork.Categories.GetByIdAsync(id);

    public async Task<IEnumerable<Category>> GetMainCategoriesAsync()
    {
        var allCategories = await _unitOfWork.Categories.GetAllAsync();
        return allCategories.Where(c => c.ParentCategoryId == null);
    }

    public async Task<IEnumerable<Category>> GetSubCategoriesByParentIdAsync(int parentId)
    {
        var allCategories = await _unitOfWork.Categories.GetAllAsync();
        return allCategories.Where(c => c.ParentCategoryId == parentId);
    }
}