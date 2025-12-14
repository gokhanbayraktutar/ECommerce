using ECommerce.Core.Entities;

namespace ECommerce.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        IRepository<Category> Categories { get; }
        IRepository<User> Users { get; }
        IRepository<Cart> Carts { get; }
        IRepository<CartItem> CartItems { get; }
        IRepository<Favourite_Product> Favourite_Products { get; }
        Task<int> CommitAsync();
    }
}
