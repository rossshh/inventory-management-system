using ims.Models;

namespace ims.DTO;

public class UserUpdateDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public UserRole? Role { get; set; }
}
