using BreakingNewsWeb.Models;
using BreakingNewsWeb.Models.TestQueryToDb;
using BreakingNewsWeb.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DBConnection.Models.Contexts;
using DBConnection.Models.Classes;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

// строки подключени€
string? connectionToUsersDb = builder.Configuration.GetConnectionString("UsersConnection");
string? connectionToEducation = builder.Configuration.GetConnectionString("EducationConnection");

// добавл€ем контексты баз данных дл€ новостей и пользователей
builder.Services.AddDbContext<NewsContext>();
//builder.Services.AddDbContext<_UsersContext>(options => options.UseNpgsql(connectionToUsersDb));
builder.Services.AddDbContext<UsersContext>();

builder.Services.AddDbContext<EducationContext>(options => options.UseNpgsql(connectionToEducation));

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
        UsersContext usersContext = new UsersContext();
        var adminRoleId = usersContext.roles.FirstOrDefault(x=>x.RoleName == "admin")?.RoleName.ToString();
        policy.RequireClaim(ClaimTypes.Role, adminRoleId);
    });
});

var app = builder.Build();
app.MapDefaultControllerRoute();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();


app.Run();
