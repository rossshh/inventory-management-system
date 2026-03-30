using ims.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ims.Repository.Interfaces;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<User> GetByUserNameAsync(string userName);
    Task<bool> ExistsByUserNameAsync(string userName);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
}