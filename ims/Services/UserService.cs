using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return _mapper.Map<UserDto?>(user);
    }

    public async Task<UserDto?> CreateAsync(RegisterDto registerDto)
    {
        var exists = await _userRepository.ExistsByUserNameAsync(registerDto.UserName);
        if (exists) return null;

        var user = _mapper.Map<User>(registerDto);
        user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);
        
        await _userRepository.AddAsync(user);

        return _mapper.Map<UserDto>(user);
    }

    public async Task UpdateAsync(int id, UserUpdateDto updateDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return;

        _mapper.Map(updateDto, user);

        if (!string.IsNullOrEmpty(updateDto.Password))
            user.PasswordHash = _passwordHasher.HashPassword(user, updateDto.Password);

        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteAsync(int id)
    {
        await _userRepository.DeleteAsync(id);
    }
}
