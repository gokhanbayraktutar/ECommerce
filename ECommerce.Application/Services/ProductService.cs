//// ProductService.cs
//using ECommerce.Core.Entities;
//using ECommerce.Core.Interfaces;
//using ECommerce.Application.Interfaces;

//namespace ECommerce.Application.Services;

//public class ProductService : IProductService
//{
//    private readonly IUnitOfWork _unitOfWork;

//    public ProductService(IUnitOfWork unitOfWork)
//    {
//        _unitOfWork = unitOfWork;
//    }

//    public async Task AddAsync(Product product) 
//    {
//        await _unitOfWork.Products.AddAsync(product);
//        await _unitOfWork.CommitAsync();
//    } 
//    public async Task DeleteAsync(int id)
//    {
//        var product = await _unitOfWork.Products.GetByIdAsync(id);
//        if (product != null) _unitOfWork.Products.Delete(product);
//        await _unitOfWork.CommitAsync();
//    }
//    public async Task<IEnumerable<Product>> GetAllAsync()
//    {
//        var products = await _unitOfWork.Products.FindAsync(
//            p => true,
//            p => p.Category
//        );

//        return products;
//    }
//    public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
//    {
//        return await _unitOfWork.Products.FindAsync(
//            p => p.CategoryId == categoryId,
//            p => p.Category
//        );
//    }
//    public async Task UpdateAsync(Product product)
//    {

//        _unitOfWork.Products.Update(product);
//        await _unitOfWork.CommitAsync();

//    }


//    public async Task<IEnumerable<Product>> SearchAsync(string query)
//    {
//        return await _unitOfWork.Products.FindAsync(
//            p => p.Name.Contains(query),
//            p => p.Category
//        );
//    }

//    public async Task<Product?> GetByIdAsync(int id)
//    {
//        var products = await _unitOfWork.Products.FindAsync(
//            p => p.Id == id,
//            p => p.Category
//        );

//        return products.FirstOrDefault();
//    }

//}
