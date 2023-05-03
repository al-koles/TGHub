using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using TGHub.Application;
using TGHub.Application.Common.Mappings;
using TGHub.Application.Services.Auth;
using TGHub.Blazor.Data;
using TGHub.Blazor.Extensions;
using TGHub.SqlDb;
using TGHub.WebApiCore;
using TGHub.WebApiCore.Controllers.Telegram;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(BotController).Assembly));

builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddTelegramBotClient();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddApplication();
builder.Services.AddSqlDb(builder.Configuration["ConnectionStrings:DefaultConnection"]);
builder.Services.AddScoped<UserSession>();
builder.Services.AddAutoMapper(opt =>
{
    opt.AddProfiles(new[]
    {
        new AssemblyMappingProfile(typeof(Program).Assembly),
        new AssemblyMappingProfile(typeof(BotController).Assembly),
        new AssemblyMappingProfile(typeof(AssemblyMappingProfile).Assembly)
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllers();

await app.MigrateDbContextIfNecessaryAsync<TgHubDbContext>();

app.Run();