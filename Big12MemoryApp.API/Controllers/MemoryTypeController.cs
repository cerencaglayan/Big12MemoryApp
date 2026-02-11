using System.Threading;
using System.Threading.Tasks;
using Big12MemoryApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Big12MemoryApp.API.Controllers;

[ApiController]
[Route("api/memoryTypes")]
[Authorize]
public class MemoryTypeController(MemoryTypeService memoryTypeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllMemoryTypes(CancellationToken ct)
    {
        var result = await memoryTypeService.getAllAsync(ct);
        return Ok(result);
    }
}