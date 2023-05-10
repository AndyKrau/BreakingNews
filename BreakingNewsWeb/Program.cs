using BreakingNewsWeb.Models;
using BreakingNewsWeb.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();


string connectionToNewsDb = builder.Configuration.GetConnectionString("NewsConnection");
string connectionToUsersDb = builder.Configuration.GetConnectionString("UsersConnection");

// добавл€ем контексты баз данных дл€ новостей и пользователей
builder.Services.AddDbContext<NewsContext>(options => options.UseNpgsql(connectionToNewsDb));
builder.Services.AddDbContext<UsersContext>(options => options.UseNpgsql(connectionToUsersDb));


// сервис добавлени€ пользовател€ в Ѕƒ
builder.Services.AddTransient<ICreateUser, CreateUser>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
                {
                    options.LoginPath = "/Home/Login";
                    options.AccessDeniedPath = "/Home/AccessDenied";
                });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlyForAdmin", policy =>
        {
            policy.RequireClaim(ClaimTypes.Role, Role.admin.ToString());
        });
});

var app = builder.Build();
app.MapDefaultControllerRoute();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
