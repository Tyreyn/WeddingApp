using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MudBlazor.Services;
using MudExtensions.Services;
using System.Security.Claims;
using WeddingApp.Components;
using WeddingApp.Controllers;
using WeddingAppBL.Repository;
using WeddingAppDTO.Context;
using WeddingAppDTO.DataTransferObject;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();
builder.Services.AddMudExtensions();
builder.Services.AddHttpClient();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddDbContext<WeddingAppUserContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("MySQL"));
    options.EnableSensitiveDataLogging();
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PictureRepository>();
builder.Services.AddScoped<PlannerRepository>();
builder.Services.AddScoped<FoodRepository>();
builder.Services.AddSingleton<CustomAuthState>();
builder.Services.AddScoped<FilesController>();
builder.Services.AddSingleton<User>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProviderController>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "auth_token";
            options.LoginPath = "/";
            options.Cookie.MaxAge = TimeSpan.FromDays(30);
            options.SlidingExpiration = true;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.AccessDeniedPath = "/Forbidden/";
            options.Cookie.HttpOnly = true;
        });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Admin");
    });
});

var app = builder.Build();
app.UseCookiePolicy();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseRouting();
app.UseAntiforgery();
app.UseAuthorization();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
                   Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = "/wwwroot"
});
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();