using Microsoft.EntityFrameworkCore;
using DBConnection.Models.Contexts;
using DBConnection.Models.Classes;
using Microsoft.AspNetCore.Identity;
using BreakingNewsWeb.Models.ViewModels;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

// добавляем контексты баз данных для новостей и пользователей
builder.Services.AddDbContext<NewsContext>();
builder.Services.AddDbContext<UsersContext>();
builder.Services.AddDbContext<ApiDataConnectionContext>();


// специфичные для Identity сервисы
builder.Services.AddIdentity<User, IdentityRole>(option =>
    {
        // корректировка правил пароля для разработки
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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await RoleInitializer.InitializeAsync(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}


app.Run();
