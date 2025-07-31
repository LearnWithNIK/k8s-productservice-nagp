using Microsoft.EntityFrameworkCore;
using ProductService.Data;

Console.WriteLine("?? ProductService v1.0 deployed and starting up!");

var host = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var user = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

Console.WriteLine($"DB_HOST: {host}");
Console.WriteLine($"DB_NAME: {dbName}");
Console.WriteLine($"DB_USER: {user}");

var connectionString = $"Host={host};Database={dbName};Username={user};Password={password};";

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Auto-create database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Enable Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
