using Microsoft.EntityFrameworkCore;
using DBConnection.Models.Contexts;
using DBConnection.Models.Classes;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

// добавляем контексты баз данных для новостей и пользователей
builder.Services.AddDbContext<NewsContext>();
builder.Services.AddDbContext<UsersContext>();

// специфичные для Identity сервисы
builder.Services.AddIdentity<User, IdentityRole>(option =>
    {
        option.Password.RequiredLength = 4;
        option.Password.RequireNonAlphanumeric = false;
        option.Password.RequireLowercase = false;
        option.Password.RequireUppercase = false;
        option.Password.RequireDigit = false;
    })
    .AddEntityFrameworkStores<UsersContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.MapDefaultControllerRoute();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
