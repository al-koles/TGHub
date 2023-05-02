namespace TGHub.Application.Services.Jwt;

public class JwtOptions
{
    public const string Alias = "JWT";

    public string Secret { get; set; } = null!;
    public int LifetimeMinutes { get; set; }
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
}