using ims.DTO;
using ims.Models;
using ims.Services.Interfaces;
using ims.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Retrieves all users (Admin only).
    /// </summary>
    /// <returns>A list of user details.</returns>
    /// <response code="200">Returns the list of users.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller is not an Admin.</response>
    [Authorize(Roles = "Admin")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    /// Retrieves a specific user by ID (Admin only).
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The user details.</returns>
    /// <response code="200">Returns the requested user.</response>
    /// <response code="404">If the user is not found.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller is not an Admin.</response>
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound(new ErrorResponse(404, "User not found"));
        return Ok(user);
    }

    /// <summary>
    /// Creates a new user (Public endpoint).
    /// </summary>
    /// <param name="registerDto">The details of the user to create.</param>
    /// <returns>The created user details.</returns>
    /// <response code="201">Returns the newly created user.</response>
    /// <response code="400">If the username is already taken.</response>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public async Task<IActionResult> Create([FromBody] RegisterDto registerDto)
    {
        var user = await _userService.CreateAsync(registerDto);
        if (user == null) return BadRequest(new ErrorResponse(400, "User already exists"));

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    /// <summary>
    /// Updates an existing user's details (Admin only).
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="updateDto">The updated information.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">If the update was successful.</response>
    /// <response code="404">If the user is not found.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller is not an Admin.</response>
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto updateDto)
    {
        var existingUser = await _userService.GetByIdAsync(id);
        if (existingUser == null) return NotFound(new ErrorResponse(404, "User not found"));

        await _userService.UpdateAsync(id, updateDto);
        return NoContent();
    }

    /// <summary>
    /// Deletes a user (Admin only).
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">If the deletion was successful.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller is not an Admin.</response>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}
