using BreakingNewsWeb.Models;
using BreakingNewsWeb.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddTransient<ITakeArticles, TakeArticles>();

var app = builder.Build();
app.MapDefaultControllerRoute();

app.Run();
