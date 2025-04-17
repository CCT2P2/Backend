using System.Text;
using Gnuf.Models;
using gnufv2.Interfaces;
using gnufv2.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"Running in {builder.Environment.EnvironmentName} mode");

// Use SQLite DB file
builder.Services.AddDbContext<GnufContext>(options =>
    options.UseSqlite("Data Source=database/GNUF.sqlite"));

builder.Services.AddControllers();

// add jwt authentication. lots of stuff to explain and i cant be bothered, just read the docs 🙏
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/configure-jwt-bearer-authentication?view=aspnetcore-9.0#jwt-bearer-token-explicit-validation
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            // this part isnt in the docs linked, its basically just verifies the signature of the token so the server knows its authentic
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ??
                                       throw new InvalidOperationException("JWT Key is not configured 2")))
        };
    });

builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine($"[{DateTime.Now}] Request: {context.Request.Method} {context.Request.Path}");
    await next();
});

app.MapControllers();

app.Run();