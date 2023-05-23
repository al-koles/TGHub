using Microsoft.Extensions.DependencyInjection;
using TGHub.Application.Interfaces;

namespace TGHub.ServerFileStorage;

public static class DependencyInjection
{
    public static void AddServerFileStorage(this IServiceCollection services)
    {
        services.AddTransient<IFileStorage, ServerFileStorageService>();
    }
}