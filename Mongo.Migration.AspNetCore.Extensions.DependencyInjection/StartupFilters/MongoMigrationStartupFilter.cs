using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mongo.Migration.AspNetCore.Extensions.DependencyInjection.StartupFilters;

internal class MongoMigrationStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return builder =>
        {
            IServiceScopeFactory factory = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using IServiceScope scope = factory.CreateScope();
            
            ILogger<MongoMigrationStartupFilter> logger = scope.ServiceProvider.GetRequiredService<ILogger<MongoMigrationStartupFilter>>();
            IMongoMigration migration = scope.ServiceProvider.GetRequiredService<IMongoMigration>();
            
            try
            {
                logger.LogInformation("Running migration. Please wait....");

                migration.Run();

                logger.LogInformation("Migration has been done");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.GetType().ToString());
            }
            
            next(builder);
        };
        
    }
}