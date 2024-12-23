using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.Documents.Locators;
using Mongo.Migration.Documents.Serializers;
using Mongo.Migration.Migrations.Adapters;
using Mongo.Migration.Migrations.Database;
using Mongo.Migration.Migrations.Document;
using Mongo.Migration.Migrations.Locators;
using Mongo.Migration.Services;
using Mongo.Migration.Services.Interceptors;

namespace Mongo.Migration.Startup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMigration(this IServiceCollection services,
        IMongoMigrationSettings settings)
    {
        services.AddSingleton(settings);

        services.AddTransient<IContainerProvider, Migrations.Adapters.ServiceProvider>();
        services.AddTransient(typeof(IMigrationLocator<>), typeof(TypeMigrationDependencyLocator<>));
        services.AddTransient<IDatabaseTypeMigrationDependencyLocator, DatabaseTypeMigrationDependencyLocator>();
        services.AddTransient<ICollectionLocator, CollectionLocator>();
        services.AddTransient<IRuntimeVersionLocator, RuntimeVersionLocator>();
        services.AddTransient<IStartUpVersionLocator, StartUpVersionLocator>();

        services.AddTransient<IDatabaseVersionService, DatabaseVersionService>();
        services.AddTransient<IDocumentVersionService, DocumentVersionService>();
        services.AddTransient<IMigrationInterceptorFactory, MigrationInterceptorFactory>();
        services.AddTransient<DocumentVersionSerializer, DocumentVersionSerializer>();

        services.AddTransient<IStartUpDocumentMigrationRunner, StartUpDocumentMigrationRunner>();
        services.AddTransient<IDocumentMigrationRunner, DocumentMigrationRunner>();

        services.AddTransient<IStartUpDatabaseMigrationRunner, StartUpDatabaseMigrationRunner>();
        services.AddTransient<IDatabaseMigrationRunner, DatabaseMigrationRunner>();

        services.AddTransient<IMigrationInterceptorProvider, MigrationInterceptorProvider>();
        
        services.AddTransient<IMigrationService, MigrationService>();
        services.AddTransient<IMongoMigration, MongoMigration>();
        
        return services;
    }
    
   
}