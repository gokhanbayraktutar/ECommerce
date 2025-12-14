using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Infrastructure.Contexts;
using ECommerce.Infrastructure.Repositories;

namespace ECommerce.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IRepository<Product> _products;
        private IRepository<Category> _categories;
        private IRepository<User> _users;
        private IRepository<Cart> _carts;
        private IRepository<CartItem> _cartItems;
        private IRepository<Favourite_Product> _favouriteProducts;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<Product> Products => _products ??= new GenericRepository<Product>(_context);
        public IRepository<Category> Categories => _categories ??= new GenericRepository<Category>(_context);
        public IRepository<User> Users => _users ??= new GenericRepository<User>(_context);
        public IRepository<Cart> Carts => _carts ??= new GenericRepository<Cart>(_context);
        public IRepository<CartItem> CartItems => _cartItems ??= new GenericRepository<CartItem>(_context);
        public IRepository<Favourite_Product> favouriteProducts => _favouriteProducts ??= new GenericRepository<Favourite_Product>(_context);

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
