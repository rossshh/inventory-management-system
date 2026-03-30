using ims.DTO;
using ims.Models;
using ims.Repository.Interfaces;
using ims.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
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

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<User?> CreateAsync(RegisterDto registerDto)
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
        return user;
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
