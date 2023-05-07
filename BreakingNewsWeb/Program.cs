using BreakingNewsWeb.Models;
using BreakingNewsWeb.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddTransient<ITakeArticles, TakeArticles>();
builder.Services.AddTransient<ITakeUsers, TakeUsers>();
builder.Services.AddTransient<ICreateUser, CreateUser>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
                {
                    o.LoginPath = "/Home/Login";
                });

var app = builder.Build();
app.MapDefaultControllerRoute();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
