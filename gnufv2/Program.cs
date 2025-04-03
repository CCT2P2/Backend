
using Gnuf.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Use SQLite DB file
builder.Services.AddDbContext<GnufContext>(options =>
    options.UseSqlite("Data Source=database/GNUF.sqlite"));

builder.Services.AddControllers();
var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine($"[{DateTime.Now}] Request: {context.Request.Method} {context.Request.Path}");
    await next();
});

app.MapControllers();
app.Run();
