using CourseworkTerm5.Server;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MainDb");

builder.Services.AddDbContext<TastyPizzaContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.MapGroup("/api").MapApi();

app.MapFallbackToFile("/index.html");

app.Run();