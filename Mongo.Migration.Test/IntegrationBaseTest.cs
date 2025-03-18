using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mongo.Migration.Documents;
using Mongo.Migration.Migrations.Database;
using Mongo.Migration.Migrations.Document;
using Mongo.Migration.Services;
using Mongo.Migration.Services.Interceptors;
using Mongo.Migration.Startup;
using Mongo.Migration.Test.Core;

using MongoDB.Driver;
using Xunit;

namespace Mongo.Migration.Test
{
    public class IntegrationBaseTest : IAsyncLifetime
    {
        private IHost _host;
        private MongoDbTestContainer _container;
        
        protected IMongoClient Client { get; private set; }
        
        protected virtual string DatabaseName { get; } = "test_db";
        
        protected IServiceProvider ServiceProvider => _host?.Services;

        public async Task InitializeAsync()
        {
            _container = new MongoDbTestContainer();
            await _container.InitializeAsync();
            
            // Create client and database
            Client = new MongoClient(_container.ConnectionString);
            
            var settings = new MongoMigrationSettings
            {
                ConnectionString = _container.ConnectionString,
                Database = DatabaseName,
            };
            
            // Configure host
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddMigration(settings);
                    
                })
                .Build();
            
            await _host.StartAsync();
        }

        public async Task DisposeAsync()
        {
            // Dispose host
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }
            
            // Dispose container
            await _container.DisposeAsync();
        }
    }
}