using ims.Models;

namespace ims.DTO;

/// <summary>
/// A data transfer object representing a user's safe profile information.
/// </summary>
public class UserDto
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The user's login name.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// The role assigned to the user (Admin, Manager, Staff).
    /// </summary>
    public UserRole Role { get; set; }
}
