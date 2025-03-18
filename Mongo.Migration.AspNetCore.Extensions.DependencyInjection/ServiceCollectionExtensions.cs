using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.AspNetCore.Extensions.DependencyInjection.StartupFilters;
using Mongo.Migration.Startup;

namespace Mongo.Migration.AspNetCore.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMigration(this IServiceCollection services,
        IMongoMigrationSettings settings)
    {
        services.AddMigration(settings);
        
        services.AddTransient<IStartupFilter, MongoMigrationStartupFilter>();
        
        return services;
        
    }
}