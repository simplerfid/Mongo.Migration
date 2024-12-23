using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.Documents;
using Mongo.Migration.Migrations.Database;
using Mongo.Migration.Startup;
using Mongo.Migration.Test.TestDoubles;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Mongo.Migration.Test.Migrations.Database
{
    
    public class DatabaseMigrationRunner_when_migrating_down : IntegrationBaseTest
    {
        private const string MigrationsCollectionName = "_migrations";
        
        protected virtual string DatabaseName { get; set; } = "DatabaseMigration";

        protected virtual string CollectionName { get; set; } = "Test";
        
        [Fact]
        public async Task When_database_has_migrations_Then_down_all_migrations()
        {
            var settings = ServiceProvider.GetRequiredService<IMongoMigrationSettings>();
            settings.DatabaseMigrationVersion = DocumentVersion.Default();
            
            // Arrange
            await this.InsertMigrations(
                new DatabaseMigration[]
                {
                    new TestDatabaseMigration_0_0_1(),
                    new TestDatabaseMigration_0_0_2(),
                    new TestDatabaseMigration_0_0_3()
                });

            // Act
            var database = Client.GetDatabase(DatabaseName);
            
            ServiceProvider.GetRequiredService<IDatabaseMigrationRunner>().Run(database);

            // Assert
            var migrations = this.GetMigrationHistory();
            migrations.Should().BeEmpty();
        }

        [Fact]
        public async Task When_database_has_migrations_Then_down_to_selected_migration()
        {
            var settings = ServiceProvider.GetRequiredService<IMongoMigrationSettings>();
            settings.DatabaseMigrationVersion = new DocumentVersion("0.0.1");

            var database = Client.GetDatabase(DatabaseName);

            // Arrange
            await this.InsertMigrations(
                new DatabaseMigration[]
                {
                    new TestDatabaseMigration_0_0_1(),
                    new TestDatabaseMigration_0_0_2(),
                    new TestDatabaseMigration_0_0_3()
                });

            // Act
            
            ServiceProvider.GetRequiredService<IDatabaseMigrationRunner>().Run(database);

            // Assert
            var migrations = this.GetMigrationHistory();
            migrations.Should().NotBeEmpty();
            migrations.Should().OnlyContain(m => m.Version == "0.0.1");
        }
        
        #region private region

        protected async Task InsertMigrations(IEnumerable<DatabaseMigration> migrations)
        {
            var database = Client.GetDatabase(DatabaseName);
            var list = migrations.Select(m => new BsonDocument { { "MigrationId", m.GetType().ToString() }, { "Version", m.Version.ToString() } });
            await database.GetCollection<BsonDocument>(MigrationsCollectionName).InsertManyAsync(list);
        }

        protected List<MigrationHistory> GetMigrationHistory()
        {
            var database = Client.GetDatabase(DatabaseName);
            var migrationHistoryCollection = database.GetCollection<MigrationHistory>(MigrationsCollectionName);
            return migrationHistoryCollection.Find(m => true).ToList();
        }

        #endregion
    }
}