using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;

namespace ECommerce.Application.Services;

public class ProductCategoryService : IProductCategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductCategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddAsync(ProductCategory productCategory)
    {
        await _unitOfWork.ProductCategories.AddAsync(productCategory);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(int productId, int categoryId)
    {
        var entity = await GetByIdAsync(productId, categoryId);
        if (entity != null)
        {
            _unitOfWork.ProductCategories.Delete(entity);
            await _unitOfWork.CommitAsync();
        }
    }

    public async Task<IEnumerable<ProductCategory>> GetAllAsync()
    {
        return await _unitOfWork.ProductCategories.GetAllAsync();
    }

    public async Task<ProductCategory?> GetByIdAsync(int productId, int categoryId)
    {
        var list = await _unitOfWork.ProductCategories.FindAsync(pc => pc.ProductId == productId && pc.CategoryId == categoryId);
        return list.FirstOrDefault();
    }

    public async Task UpdateAsync(ProductCategory productCategory)
    {
        _unitOfWork.ProductCategories.Update(productCategory);
        await _unitOfWork.CommitAsync();
    }
}