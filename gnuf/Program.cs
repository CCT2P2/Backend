var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; 
});

var app = builder.Build(); //make sure the header shows the correct  {"Content-Type": "application/json"} and {"Accept": "application/json"}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();