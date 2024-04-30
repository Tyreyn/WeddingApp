using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.FileProviders;
using WeddingApp.Components;
using WeddingApp.Context;
using WeddingApp.Controllers;
using WeddingApp.Entities;
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets("ConnectionStringClass");

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<CustomAuthState>();
builder.Services.AddSingleton<SqlServerDataAccess>();
builder.Services.AddSingleton<SqlServerDataController>();
builder.Services.AddSingleton<FilesController>();
builder.Services.AddSingleton<UserEntity>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProviderController>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "auth_token";
            options.LoginPath = "/login";
            options.Cookie.MaxAge = TimeSpan.FromDays(30);
            options.SlidingExpiration = true;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.AccessDeniedPath = "/Forbidden/";
            options.Cookie.HttpOnly = true;
        });
builder.Services.AddAuthorization();
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
//app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
                   Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = "/wwwroot"
});
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();