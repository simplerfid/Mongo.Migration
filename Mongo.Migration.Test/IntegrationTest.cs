using System;
using System.Threading.Tasks;
using Mongo.Migration.Startup;
using Mongo.Migration.Startup.Static;
using Mongo.Migration.Test.Core;

using MongoDB.Driver;
using Xunit;

namespace Mongo.Migration.Test
{
    public class IntegrationTest : IAsyncLifetime
    {
        protected IMongoClient _client;

        protected IComponentRegistry _components;

        protected MongoDbTestContainer _container;

        public async Task InitializeAsync()
        {
            _container = new MongoDbTestContainer();
            await _container.InitializeAsync();
            _client = new MongoClient(_container.ConnectionString);

            _client.GetDatabase("PerformanceTest").CreateCollection("Test");

            _components = new ComponentRegistry(
                new MongoMigrationSettings
                    { ConnectionString = _container.ConnectionString, Database = "PerformanceTest" });
            _components.RegisterComponents(this._client);
        }

        public async Task DisposeAsync()
        {
            await _container?.DisposeAsync();
        }
    }
}