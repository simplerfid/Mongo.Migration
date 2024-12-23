using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mongo.Migration.Documents;
using Mongo.Migration.Migrations.Database;
using Mongo.Migration.Startup;
using Mongo.Migration.Startup.Static;
using Mongo.Migration.Test.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Mongo.Migration.Test.Migrations.Database
{
    internal class DatabaseIntegrationTest: IAsyncLifetime
    {
        private const string MigrationsCollectionName = "_migrations";

        protected IMongoClient _client;

        protected IComponentRegistry _components;

        protected IMongoDatabase _db;
        
        protected MongoDbTestContainer _container;
        
        protected virtual string DatabaseName { get; set; } = "DatabaseMigration";

        protected virtual string CollectionName { get; set; } = "Test";
        
        protected virtual async Task OnSetUpAsync(DocumentVersion databaseMigrationVersion)
        {

            this._components = new ComponentRegistry(
                new MongoMigrationSettings
                {
                    ConnectionString = _container.ConnectionString,
                    Database = this.DatabaseName,
                    DatabaseMigrationVersion = databaseMigrationVersion
                });
            this._components.RegisterComponents(this._client);
        }

        protected void InsertMigrations(IEnumerable<DatabaseMigration> migrations)
        {
            var list = migrations.Select(m => new BsonDocument { { "MigrationId", m.GetType().ToString() }, { "Version", m.Version.ToString() } });
            this._db.GetCollection<BsonDocument>(MigrationsCollectionName).InsertManyAsync(list).Wait();
        }

        protected List<MigrationHistory> GetMigrationHistory()
        {
            var migrationHistoryCollection = this._db.GetCollection<MigrationHistory>(MigrationsCollectionName);
            return migrationHistoryCollection.Find(m => true).ToList();
        }

        public async Task InitializeAsync()
        {
            _container = new MongoDbTestContainer();
            await _container.InitializeAsync();
            this._client = new MongoClient(_container.ConnectionString);
            this._db = this._client.GetDatabase(this.DatabaseName);
            this._db.CreateCollection(this.CollectionName);
        }

        public async Task DisposeAsync()
        {
            await _container.DisposeAsync();
        }
    }
}