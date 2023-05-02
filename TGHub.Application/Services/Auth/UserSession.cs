using System.Security.Claims;

namespace TGHub.Application.Services.Auth;

public class UserSession
{
    public int Id { get; set; }
    public string TelegramId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? UserName { get; set; }

    public string? PhotoUrl { get; set; }

    public string? AuthDate { get; set; }

    public List<Claim> ToClaims()
    {
        var claims = new List<Claim>
        {
            new(nameof(Id), Id.ToString()),
            new(nameof(TelegramId), TelegramId),
            new(nameof(UserName), UserName ?? ""),
            new(nameof(FirstName), FirstName ?? ""),
            new(nameof(LastName), LastName ?? ""),
            new(nameof(PhotoUrl), PhotoUrl ?? ""),
            new(nameof(AuthDate), AuthDate ?? "")
        };
        return claims;
    }

    public UserSession FillWithClaims(List<Claim> claims)
    {
        Id = int.Parse(claims.FirstOrDefault(c => c.Type == nameof(Id))?.Value ?? "0");
        TelegramId = claims.FirstOrDefault(c => c.Type == nameof(TelegramId))?.Value ?? string.Empty;
        FirstName = claims.FirstOrDefault(c => c.Type == nameof(FirstName))?.Value;
        LastName = claims.FirstOrDefault(c => c.Type == nameof(LastName))?.Value;
        UserName = claims.FirstOrDefault(c => c.Type == nameof(UserName))?.Value;
        AuthDate = claims.FirstOrDefault(c => c.Type == nameof(AuthDate))?.Value;
        PhotoUrl = claims.FirstOrDefault(c => c.Type == nameof(PhotoUrl))?.Value;
        return this;
    }

    public void CopyTo(UserSession userSession)
    {
        userSession.Id = Id;
        userSession.TelegramId = TelegramId;
        userSession.FirstName = FirstName;
        userSession.LastName = LastName;
        userSession.UserName = UserName;
        userSession.PhotoUrl = PhotoUrl;
        userSession.AuthDate = AuthDate;
    }
}