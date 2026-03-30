using ims.DTO;
using ims.Models;
using System.Threading.Tasks;

namespace ims.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> AuthenticateAsync(LoginDto login);
    Task<User> RegisterAsync(RegisterDto registerDto);
    Task SeedDefaultAdminAsync();
}