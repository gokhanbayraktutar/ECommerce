using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddAsync(User user)
    {
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.CommitAsync();  // ✔ kayıt burada yapılır
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user != null)
            _unitOfWork.Users.Delete(user);

        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<User>> GetAllAsync() => await _unitOfWork.Users.GetAllAsync();

    public async Task<User> GetByIdAsync(int id) => await _unitOfWork.Users.GetByIdAsync(id);

    public async Task UpdateAsync(User user)
    {
        _unitOfWork.Users.Update(user);
        await _unitOfWork.CommitAsync();  // ✔ güncellemede de gerekli
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        var users = await _unitOfWork.Users.FindAsync(u => u.Username == username);
        return users.FirstOrDefault();
    }
}
