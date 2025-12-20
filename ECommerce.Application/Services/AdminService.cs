using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;

public class AdminService : IAdminService
{
    private readonly IUnitOfWork _unitOfWork;

    public AdminService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddAsync(Admin Admin)
    {
        await _unitOfWork.Admins.AddAsync(Admin);
        await _unitOfWork.CommitAsync();  
    }

    public async Task DeleteAsync(int id)
    {
        var Admin = await _unitOfWork.Admins.GetByIdAsync(id);
        if (Admin != null)
            _unitOfWork.Admins.Delete(Admin);

        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<Admin>> GetAllAsync() => await _unitOfWork.Admins.GetAllAsync();

    public async Task<Admin> GetByIdAsync(int id) => await _unitOfWork.Admins.GetByIdAsync(id);

    public async Task UpdateAsync(Admin Admin)
    {
        _unitOfWork.Admins.Update(Admin);
        await _unitOfWork.CommitAsync();  
    }

    public async Task<Admin> GetByUsernameAsync(string username)
    {
        var users = await _unitOfWork.Admins.FindAsync(u => u.Username == username);
        return users.FirstOrDefault();
    }
}
