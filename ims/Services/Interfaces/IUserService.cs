using ims.DTO;
using ims.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto?> CreateAsync(RegisterDto registerDto);
    Task UpdateAsync(int id, UserUpdateDto userUpdateDto);
    Task DeleteAsync(int id);
}
