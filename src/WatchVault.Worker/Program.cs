using Microsoft.EntityFrameworkCore;
using WatchVault.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<WatchVaultDbContext>(options => 
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string not configured")));

builder.Services.AddScoped<IMediaRepository, MediaRepository>();

var host = builder.Build();
host.Run();
