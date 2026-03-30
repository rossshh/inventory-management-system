using ims.DTO;
using ims.Helpers;
using ims.Models;
using ims.Repository.Interfaces;
using ims.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ims.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettings.Value;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<AuthResponseDto> AuthenticateAsync(LoginDto login)
    {
        var user = await _userRepository.GetByUserNameAsync(login.UserName);

        if (user == null)
            return null;

        var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);
        if (verifyResult == PasswordVerificationResult.Failed)
            return null;

        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            UserName = user.UserName,
            Role = user.Role.ToString()
        };
    }

    public async Task<User> RegisterAsync(RegisterDto registerDto)
    {
        var exists = await _userRepository.ExistsByUserNameAsync(registerDto.UserName);
        if (exists)
            return null;

        var user = new User
        {
            UserName = registerDto.UserName,
            Role = registerDto.Role
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

        await _userRepository.AddAsync(user);

        return user;
    }

    public async Task SeedDefaultAdminAsync()
    {
        var adminExists = await _userRepository.ExistsByUserNameAsync("admin");
        if (!adminExists)
        {
            var adminUser = new User
            {
                UserName = "admin",
                Role = UserRole.Admin
            };
            adminUser.PasswordHash = _passwordHasher.HashPassword(adminUser, "Admin123!");
            await _userRepository.AddAsync(adminUser);
        }
    }

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }
}