namespace ims.DTO;

/// <summary>
/// Represents the response returned after a successful authentication.
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// The JWT bearer token to be used for subsequent authorized requests.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// The expiration date and time of the token.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// The username of the authenticated user.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// The role assigned to the user.
    /// </summary>
    public string Role { get; set; } = string.Empty;
}