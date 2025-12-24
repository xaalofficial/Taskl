namespace ProjectTaskManager.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using ProjectTaskManager.Application.DTOs.Auth;
using ProjectTaskManager.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _authService.LoginAsync(request);

        if (response == null)
            return Unauthorized(new { message = "Invalid email or password" });

        return Ok(response);
    }
}