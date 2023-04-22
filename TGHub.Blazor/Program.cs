using Microsoft.AspNetCore.Mvc.ApplicationParts;
using TGHub.Blazor.Data;
using TGHub.Blazor.Extensions;
using TGHub.WebApiCore;
using TGHub.WebApiCore.Controllers.Telegram;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddTelegramBotClient();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TelegramController).Assembly));
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllers();

app.Run();