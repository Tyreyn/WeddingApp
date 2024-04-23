using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
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
builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<CustomAuthState>();
builder.Services.AddSingleton<SqlServerDataAccess>();
builder.Services.AddSingleton<SqlServerDataController>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProviderController>();
builder.Services.AddScoped<UserEntity>();
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
app.UseStaticFiles();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();