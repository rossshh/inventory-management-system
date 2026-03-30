using ims.Models;

namespace ims.DTO;

public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public UserRole Role { get; set; }
}
