using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;

namespace ECommerce.Application.Services;

public class Favourite_ProductService : IFavourite_ProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public Favourite_ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddAsync(Favourite_Product favourite_Product)
    {
        await _unitOfWork.Favourite_Products.AddAsync(favourite_Product);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var favourite_product = await _unitOfWork.Favourite_Products.GetByIdAsync(id);
        if (favourite_product != null)
        {
            _unitOfWork.Favourite_Products.Delete(favourite_product);
            await _unitOfWork.CommitAsync();
        }
    }

    public async Task<IEnumerable<Favourite_Product>> GetAllAsync() => await _unitOfWork.Favourite_Products.GetAllAsync();

    public async Task<IEnumerable<Favourite_Product>> GetByUserIdAsync(int userId) =>
    await _unitOfWork.Favourite_Products.FindAsync(fp => fp.UserId == userId);


}
