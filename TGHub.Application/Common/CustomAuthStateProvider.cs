using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using TGHub.Application.Common.SessionStorage;
using TGHub.Application.Services.Jwt;

namespace TGHub.Application.Common;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private const string TokenKey = "Auth";
    private readonly IJwtService _jwtService;
    private readonly LocalStorageProvider _localStorageProvider;
    private readonly ILocalStorageService _localStorageService;
    private readonly SessionStorageProvider _sessionStorageProvider;

    public CustomAuthStateProvider(ILocalStorageService localStorageService, IJwtService jwtService,
        LocalStorageProvider localStorageProvider, SessionStorageProvider sessionStorageProvider)
    {
        _localStorageService = localStorageService;
        _jwtService = jwtService;
        _localStorageProvider = localStorageProvider;
        _sessionStorageProvider = sessionStorageProvider;
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
        userSession.CopyTo(_localStorageProvider);

        await _sessionStorageProvider.FetchAsync();

        var identity = new ClaimsIdentity(claims, "auth");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task LoginAsync(LocalStorageProvider localStorageProvider)
    {
        var token = _jwtService.GenerateToken(localStorageProvider);
        await _localStorageService.SetItemAsStringAsync(TokenKey, token);
        var identity = new ClaimsIdentity(localStorageProvider.ToClaims(), "auth");
        var principal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public async Task LogoutAsync()
    {
        await _localStorageService.RemoveItemAsync(TokenKey);
        NotifyAuthenticationStateChanged(Task.FromResult(NotAuthenticated));
    }
}