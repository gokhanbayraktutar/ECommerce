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

    public async Task AddAsync(Category category) => await _unitOfWork.Categories.AddAsync(category);

    public async Task DeleteAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category != null)
            _unitOfWork.Categories.Delete(category);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<Category>> GetAllAsync() => await _unitOfWork.Categories.GetAllAsync();

    public async Task<Category> GetByIdAsync(int id) => await _unitOfWork.Categories.GetByIdAsync(id);

    public async Task UpdateAsync(Category category) => _unitOfWork.Categories.Update(category);
}
