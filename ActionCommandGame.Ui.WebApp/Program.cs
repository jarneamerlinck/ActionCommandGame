
using System.Net;
using ActionCommandGame.Sdk;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Ui.WebApp;
using ActionCommandGame.Ui.WebApp.Models;
using ActionCommandGame.Ui.WebApp.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);


var appSettings = new AppSettings();
builder.Configuration.GetSection(nameof(AppSettings)).Bind(appSettings);
builder.Services.AddHttpContextAccessor();
builder.Services.AddApi(appSettings.ApiBaseUrl);


builder.Services.AddTransient<ITokenStore, TokenStore>();
builder.Services.AddTransient<IPlayerStore, PlayerStore>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config => {
        config.AccessDeniedPath = appSettings.SignInUrl;
        config.LoginPath = appSettings.SignInUrl;
        //config.SlidingExpiration = true;
        config.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        

    });
/*
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("player", policy =>
    {
        policy.AuthenticationSchemes.Add(CookieAuthenticationDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});*/
// Add services to the container.
builder.Services.AddControllersWithViews();




var app = builder.Build();

// Configure the HTTP request pipeline.
var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Unspecified,
    HttpOnly = HttpOnlyPolicy.Always,
    

};

app.UseCookiePolicy(cookiePolicyOptions);
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
