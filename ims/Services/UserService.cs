using ims.DTO;
using ims.Models;
using ims.Repository.Interfaces;
using ims.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace ims.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserDto
        {
            Id = u.Id,
            UserName = u.UserName,
            Role = u.Role
        });
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var u = await _userRepository.GetByIdAsync(id);
        if (u == null) return null;

        return new UserDto
        {
            Id = u.Id,
            UserName = u.UserName,
            Role = u.Role
        };
    }

    public async Task<UserDto?> CreateAsync(RegisterDto registerDto)
    {
        var exists = await _userRepository.ExistsByUserNameAsync(registerDto.UserName);
        if (exists) return null;

        var user = new User
        {
            UserName = registerDto.UserName,
            Role = registerDto.Role
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);
        await _userRepository.AddAsync(user);

        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Role = user.Role
        };
    }

    public async Task UpdateAsync(int id, UserUpdateDto updateDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return;

        if (!string.IsNullOrEmpty(updateDto.UserName))
            user.UserName = updateDto.UserName;

        if (!string.IsNullOrEmpty(updateDto.Password))
            user.PasswordHash = _passwordHasher.HashPassword(user, updateDto.Password);

        if (updateDto.Role.HasValue)
            user.Role = updateDto.Role.Value;

        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteAsync(int id)
    {
        await _userRepository.DeleteAsync(id);
    }
}
