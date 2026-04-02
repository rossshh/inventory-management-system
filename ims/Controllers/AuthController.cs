using ims.DTO;
using ims.Services.Interfaces;
using ims.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ims.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="loginDto">The user's login credentials.</param>
    /// <returns>An AuthResponseDto containing the JWT token.</returns>
    /// <response code="200">Returns the JWT token and user info.</response>
    /// <response code="401">If the credentials are invalid.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var response = await _authService.AuthenticateAsync(loginDto);
        if (response == null)
            return Unauthorized(new ErrorResponse(401, "Invalid username or password"));

        return Ok(response);
    }

    /// <summary>
    /// Registers a new user (Admin only).
    /// </summary>
    /// <param name="registerDto">The new user's details.</param>
    /// <returns>The newly created user.</returns>
    /// <response code="201">Returns the newly created user.</response>
    /// <response code="400">If the user already exists or data is invalid.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller is not an Admin.</response>
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDto), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var user = await _authService.RegisterAsync(registerDto);
        if (user == null)
            return BadRequest(new ErrorResponse(400, "User already exists"));

        return CreatedAtAction(null, new { id = user.Id }, user);
    }
}