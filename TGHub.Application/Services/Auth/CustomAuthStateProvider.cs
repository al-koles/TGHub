using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using TGHub.Application.Services.Jwt;

namespace TGHub.Application.Services.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private const string TokenKey = "Auth";
    private readonly IJwtService _jwtService;
    private readonly ILocalStorageService _localStorageService;
    private readonly UserSession _userSession;

    public CustomAuthStateProvider(ILocalStorageService localStorageService, IJwtService jwtService,
        UserSession userSession)
    {
        _localStorageService = localStorageService;
        _jwtService = jwtService;
        _userSession = userSession;
    }

    private AuthenticationState NotAuthenticated => new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorageService.GetItemAsStringAsync(TokenKey);
        if (token == null)
        {
            return NotAuthenticated;
        }

        var isTokenValid = _jwtService.ValidateToken(token);
        if (!isTokenValid)
        {
            return NotAuthenticated;
        }

        var (userSession, claims) = _jwtService.ParseToken(token);
        userSession.CopyTo(_userSession);
        var identity = new ClaimsIdentity(claims, "auth");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task LoginAsync(UserSession userSession)
    {
        var token = _jwtService.GenerateToken(userSession);
        await _localStorageService.SetItemAsStringAsync(TokenKey, token);
        var identity = new ClaimsIdentity(userSession.ToClaims(), "auth");
        var principal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public async Task LogoutAsync()
    {
        await _localStorageService.RemoveItemAsync(TokenKey);
        NotifyAuthenticationStateChanged(Task.FromResult(NotAuthenticated));
    }
}