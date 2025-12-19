using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Big12MemoryApp.Application.DTO.Requests;
using Big12MemoryApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Big12MemoryApp.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(TokenService tokenService, UserService userService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await userService.Login(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Problem(result.Error.Message);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await userService.Logout(userId);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }
}