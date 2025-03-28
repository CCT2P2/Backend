using database;

//change in future, current approach allows all origins, methods, and headers
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});


builder.Services.AddSingleton<Database>();

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowAll"); // Add this
app.UseAuthorization();
app.MapControllers();

app.Run();
