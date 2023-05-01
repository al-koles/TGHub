using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TGHub.WebApiCore.Controllers.Auth.Models;
using TGHub.WebApiCore.Options;

namespace TGHub.WebApiCore.Controllers.Auth;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly TelegramBotOptions _telegramBotOptions;

    public AuthController(IOptionsSnapshot<TelegramBotOptions> telegramBotOptions)
    {
        _telegramBotOptions = telegramBotOptions.Value;
    }
    
    [HttpGet]
    public async Task<IActionResult> LoginCallback(string? redirectUrl, [FromQuery] LoginCallbackRequest request)
    {
        if (IsLoginValid(request.Hash))
        {
            await AuthenticateUser(request.Id);
            return Redirect("/" + redirectUrl ?? "");
        }
        else
        {
            return Redirect("TelegramLogin");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string? redirectUrl)
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect($"/login?redirectUrl={redirectUrl}");
    }

    private bool IsLoginValid(string hash)
    {
        var query = HttpContext.Request.Query;

        var dataCheckString = string.Join("\n",
            query.Where(p => p.Key != "hash" && p.Key != "redirectUrl")
                .Select(p => $"{p.Key}={p.Value}")
                .OrderBy(p => p));
        var secretKey = SHA256.HashData(Encoding.UTF8.GetBytes(_telegramBotOptions.Token));
        var checkHash = BitConverter.ToString(HMACSHA256.HashData(secretKey, Encoding.UTF8.GetBytes(dataCheckString)))
            .Replace("-", "").ToLower();

        return checkHash == hash;
    }
    
    private async Task AuthenticateUser(int id)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString())
        }, "auth");
        var user = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
    }
}