using Microsoft.EntityFrameworkCore;
using DBConnection.Models.Contexts;
using DBConnection.Models.Classes;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

// ��������� ��������� ��� ������ ��� �������� � �������������
builder.Services.AddDbContext<NewsContext>();
builder.Services.AddDbContext<UsersContext>();

// ����������� ��� Identity �������
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<UsersContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.MapDefaultControllerRoute();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
