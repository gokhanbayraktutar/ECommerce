using ECommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IFavourite_ProductService
    {
        Task<IEnumerable<Favourite_Product>> GetAllAsync();
        Task<IEnumerable<Favourite_Product>> GetByUserIdAsync(int userId);
        Task AddAsync(Favourite_Product category);
        Task DeleteAsync(int id);

    }
}
