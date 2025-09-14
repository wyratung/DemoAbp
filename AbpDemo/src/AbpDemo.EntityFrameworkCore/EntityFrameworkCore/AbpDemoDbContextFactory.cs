using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AbpDemo.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class AbpDemoDbContextFactory : IDesignTimeDbContextFactory<AbpDemoDbContext>
{
    public AbpDemoDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        AbpDemoEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<AbpDemoDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new AbpDemoDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AbpDemo.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
