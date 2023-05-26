using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.SessionStorage;
using Blazored.Toast;
using TGHub.Application;
using TGHub.Application.Common.Mappings;
using TGHub.Blazor.Data;
using TGHub.Blazor.Extensions;
using TGHub.ServerFileStorage;
using TGHub.SqlDb;
using TGHub.Telegram.Bot;
using TGHub.WebApiCore.Controllers.Culture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .AddApplicationPart(typeof(BotController).Assembly)
    .AddApplicationPart(typeof(CultureController).Assembly);

builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddTelegramBotClient();
builder.Services.AddServerFileStorage();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredModal();
builder.Services.AddApplication();
builder.Services.AddSqlDb(builder.Configuration["ConnectionStrings:DefaultConnection"]);
builder.Services.AddAutoMapper(opt =>
{
    opt.AddProfiles(new[]
    {
        new AssemblyMappingProfile(typeof(Program).Assembly),
        new AssemblyMappingProfile(typeof(BotController).Assembly),
        new AssemblyMappingProfile(typeof(AssemblyMappingProfile).Assembly)
    });
});
builder.Services.AddLocalization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();
app.UseLocalization();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllers();

await app.MigrateDbContextIfNecessaryAsync<TgHubDbContext>();
await app.SchedulePostsAsync();
await app.ScheduleLotteriesAsync();

app.Run();