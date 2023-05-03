using Microsoft.Extensions.DependencyInjection;
using TGHub.Application.Interfaces;

namespace TGHub.SqlDb;

public static class DependencyInjection
{
    public static IServiceCollection AddSqlDb(this IServiceCollection services, string connectionString)
    {
        services.AddSqlServer<TgHubDbContext>(connectionString);
        services.AddScoped<ITgHubDbContext>(provider =>
            provider.GetRequiredService<TgHubDbContext>());

        return services;
    }
}