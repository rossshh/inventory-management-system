using ims.DTO;
using ims.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> CreateAsync(RegisterDto registerDto);
    Task UpdateAsync(int id, UserUpdateDto userUpdateDto);
    Task DeleteAsync(int id);
}
