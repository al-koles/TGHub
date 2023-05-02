using System.Security.Claims;
using TGHub.Application.Services.Auth;

namespace TGHub.Application.Services.Jwt;

public interface IJwtService
{
    string GenerateToken(UserSession userSession);
    (UserSession, List<Claim>) ParseToken(string token);
    bool ValidateToken(string token);
}