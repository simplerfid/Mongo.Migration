using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.Migrations.Database;
using Mongo.Migration.Test.TestDoubles;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Mongo.Migration.Test.Migrations.Database
{
    
    public class DatabaseMigrationRunner_when_migrating_up : IntegrationBaseTest
    {
        private const string MigrationsCollectionName = "_migrations";
        
        protected virtual string DatabaseName { get; set; } = "DatabaseMigration";

        protected virtual string CollectionName { get; set; } = "Test";
        
        [Fact]
        public async Task When_database_has_no_migrations_Then_all_migrations_are_used()
        {
            // Act

            var database = Client.GetDatabase(DatabaseName);
            
            ServiceProvider.GetRequiredService<IDatabaseMigrationRunner>().Run(database);

            // Assert
            var migrations = this.GetMigrationHistory();
            migrations.Should().NotBeEmpty();
            migrations[0].Version.ToString().Should().BeEquivalentTo("0.0.1");
            migrations[1].Version.ToString().Should().BeEquivalentTo("0.0.2");
            migrations[2].Version.ToString().Should().BeEquivalentTo("0.0.3");
        }

        [Fact]
        public async Task When_database_has_migrations_Then_latest_migrations_are_used()
        {
            // Arrange
            await this.InsertMigrations(new DatabaseMigration[] { new TestDatabaseMigration_0_0_1(), new TestDatabaseMigration_0_0_2() });

            var database = Client.GetDatabase(DatabaseName);
            
            // Act
            ServiceProvider.GetRequiredService<IDatabaseMigrationRunner>().Run(database);

            // Assert
            var migrations = this.GetMigrationHistory();
            migrations.Should().NotBeEmpty();
            migrations[2].Version.ToString().Should().BeEquivalentTo("0.0.3");
        }

        [Fact]
        public async Task When_database_has_latest_version_Then_nothing_happens()
        {
            // Arrange
            
            var database = Client.GetDatabase(DatabaseName);
            
            await this.InsertMigrations(
                new DatabaseMigration[] { new TestDatabaseMigration_0_0_1(), new TestDatabaseMigration_0_0_2(), new TestDatabaseMigration_0_0_3() });

            // Act
            
            ServiceProvider.GetRequiredService<IDatabaseMigrationRunner>().Run(database);

            // Assert
            var migrations = this.GetMigrationHistory();
            migrations.Should().NotBeEmpty();
            migrations[0].Version.ToString().Should().BeEquivalentTo("0.0.1");
            migrations[1].Version.ToString().Should().BeEquivalentTo("0.0.2");
            migrations[2].Version.ToString().Should().BeEquivalentTo("0.0.3");
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