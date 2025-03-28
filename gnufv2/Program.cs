
using Gnuf.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Use SQLite DB file
builder.Services.AddDbContext<GnufContext>(options =>
    options.UseSqlite("Data Source=../data/GNUF.sqlite"));

builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();
