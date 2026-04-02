using ims.DTO;
using ims.Models;
using System.Threading.Tasks;

namespace ims.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> AuthenticateAsync(LoginDto login);
    Task<UserDto> RegisterAsync(RegisterDto registerDto);
    Task SeedDefaultAdminAsync();
}