using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TGHub.Application.Common;

namespace TGHub.Application.Services.Jwt;

public class JwtService : IJwtService
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(IOptionsSnapshot<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(LocalStorageProvider localStorageProvider)
    {
        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: localStorageProvider.ToClaims(),
            notBefore: now,
            expires: now + TimeSpan.FromMinutes(_jwtOptions.LifetimeMinutes),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public (LocalStorageProvider, List<Claim>) ParseToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var parsedToken = tokenHandler.ReadJwtToken(token);
        var userSession = new LocalStorageProvider().FillWithClaims(parsedToken.Claims.ToList());

        return (userSession, parsedToken.Claims.ToList());
    }

    public bool ValidateToken(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
            ValidateIssuer = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtOptions.Audience
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal.Identity is { IsAuthenticated: true };
        }
        catch
        {
            return false;
        }
    }
}