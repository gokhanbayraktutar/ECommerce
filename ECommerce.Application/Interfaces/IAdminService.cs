using ECommerce.Core.Entities;

namespace ECommerce.Application.Interfaces;

public interface IAdminService
{
    Task<IEnumerable<Admin>> GetAllAsync();
    Task<Admin> GetByIdAsync(int id);
    Task AddAsync(Admin user);
    Task UpdateAsync(Admin user);
    Task DeleteAsync(int id);
    Task<Admin> GetByUsernameAsync(string username);
}
