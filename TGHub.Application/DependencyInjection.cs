﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using TGHub.Application.Services.Auth;
using TGHub.Application.Services.Jwt;

namespace TGHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IJwtService, JwtService>();
        services.AddScoped<UserSession>();
        services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        return services;
    }
}