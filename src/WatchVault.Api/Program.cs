using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Polly;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"The Environment is '{builder.Environment.EnvironmentName}'");

// Database
builder.Services.AddDbContext<WatchVaultDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// TMDB
if (!builder.Environment.IsEnvironment("Test"))
{
    var tmdbApiKey = builder.Configuration["Tmdb:ApiKey"]
        ?? throw new InvalidOperationException("TMDB API key not configured");

    builder.Services.AddHttpClient<TmdbClient>(client =>
    {
        client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tmdbApiKey);
    })
    .AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(3, attempt =>
            TimeSpan.FromSeconds(Math.Pow(2, attempt))));

}

// Jikan
if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddHttpClient<JikanClient>(client =>
    {
        client.BaseAddress = new Uri("https://api.jikan.moe/v4/");
    })
    .AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(3, attempt =>
            TimeSpan.FromSeconds(Math.Pow(2, attempt))));
}

// Redis
if (!builder.Environment.IsEnvironment("Test"))
{
    var redisConnection = builder.Configuration["Redis:ConnectionString"]
    ?? throw new InvalidOperationException("Redis Connection String not configured");

    builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
}

// Repositories
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<IWatchEntryRepository, WatchEntryRepository>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }