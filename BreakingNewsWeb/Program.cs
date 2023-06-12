using Microsoft.EntityFrameworkCore;
using DBConnection.Models.Contexts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

// добавляем контексты баз данных для новостей и пользователей
builder.Services.AddDbContext<NewsContext>();
//builder.Services.AddDbContext<_UsersContext>(options => options.UseNpgsql(connectionToUsersDb));
builder.Services.AddDbContext<UsersContext>();

var app = builder.Build();
app.MapDefaultControllerRoute();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
