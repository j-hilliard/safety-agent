using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Stronghold.AppDashboard.Data;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new ConfigurationBuilder().AddUserSecrets<DesignTimeContextFactory>();
        var config = builder.Build();
        var connectionString = config.GetConnectionString("SqlDb");
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(
            connectionString
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}
