using System.Security.Claims;
using TGHub.Application.Common;

namespace TGHub.Application.Services.Jwt;

public interface IJwtService
{
    string GenerateToken(LocalStorageProvider localStorageProvider);
    (LocalStorageProvider, List<Claim>) ParseToken(string token);
    bool ValidateToken(string token);
}