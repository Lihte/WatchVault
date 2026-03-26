using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class WatchVaultDbContextFactory : IDesignTimeDbContextFactory<WatchVaultDbContext>
{
    public WatchVaultDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WatchVaultDbContext>();
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=watchvault;Username=watchvault;Password=watchvault_dev");

        return new WatchVaultDbContext(optionsBuilder.Options);
    }
}