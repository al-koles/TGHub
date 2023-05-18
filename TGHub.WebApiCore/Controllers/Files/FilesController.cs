using Microsoft.AspNetCore.Mvc;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Jwt;

namespace TGHub.WebApiCore.Controllers.Files;

[ApiController]
[Route("api/[controller]")]
public class FilesController : Controller
{
    private readonly IFileStorage _fileStorage;
    private readonly IJwtService _jwtService;

    public FilesController(IFileStorage fileStorage, IJwtService jwtService)
    {
        _fileStorage = fileStorage;
        _jwtService = jwtService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFile(string fileName, string directory, string token)
    {
        if (!_jwtService.ValidateToken(token))
        {
            return BadRequest("Token not valid");
        }

        try
        {
            var file = await _fileStorage.DownloadAsync(fileName, directory);
            return Ok(file);
        }
        catch
        {
            return NotFound();
        }
    }
}