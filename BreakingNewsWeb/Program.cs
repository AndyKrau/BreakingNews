using BreakingNewsWeb.Models;
using BreakingNewsWeb.Models.TestQueryToDb;
using BreakingNewsWeb.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DBConnection.Models.Contexts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

// ������ �����������
string? connectionToUsersDb = builder.Configuration.GetConnectionString("UsersConnection");
string? connectionToEducation = builder.Configuration.GetConnectionString("EducationConnection");

// ��������� ��������� ��� ������ ��� �������� � �������������
builder.Services.AddDbContext<NewsContext>();
builder.Services.AddDbContext<UsersContext>(options => options.UseNpgsql(connectionToUsersDb));
builder.Services.AddDbContext<EducationContext>(options => options.UseNpgsql(connectionToEducation));

// ������ ���������� ������������ � ��
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
