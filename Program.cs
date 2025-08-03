using Microsoft.EntityFrameworkCore;
using Polly;
using ProductService.Data;

var host = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var user = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

var connectionString = $"Host={host};Database={dbName};Username={user};Password={password};";

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Auto-create database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetry(
            retryCount: 5,
            sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(5, attempt)),
            onRetry: (exception, timeSpan, retryCount, context) => {});

    retryPolicy.Execute(() =>
    {
        db.Database.EnsureCreated();
        Console.WriteLine("✅ Database schema ready.");
    });
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
