using System.Threading.Tasks;
using FluentAssertions;

using Mongo.Migration.Documents;
using Mongo.Migration.Migrations.Database;
using Mongo.Migration.Test.TestDoubles;
using Xunit;

namespace Mongo.Migration.Test.Migrations.Database
{
    
    internal class DatabaseMigrationRunner_when_migrating_up : DatabaseIntegrationTest
    {
        private IDatabaseMigrationRunner _runner;

        
        public async Task SetUpAsync()
        {
            await base.OnSetUpAsync(DocumentVersion.Empty());

            this._runner = this._components.Get<IDatabaseMigrationRunner>();
        }

        
        public async Task TearDown()
        {
            await this.DisposeAsync();
        }

        [Fact]
        public void When_database_has_no_migrations_Then_all_migrations_are_used()
        {
            // Act
            this._runner.Run(this._db);

            // Assert
            var migrations = this.GetMigrationHistory();
            migrations.Should().NotBeEmpty();
            migrations[0].Version.ToString().Should().BeEquivalentTo("0.0.1");
            migrations[1].Version.ToString().Should().BeEquivalentTo("0.0.2");
            migrations[2].Version.ToString().Should().BeEquivalentTo("0.0.3");
        }

        [Fact]
        public void When_database_has_migrations_Then_latest_migrations_are_used()
        {
            // Arrange
            this.InsertMigrations(new DatabaseMigration[] { new TestDatabaseMigration_0_0_1(), new TestDatabaseMigration_0_0_2() });

            // Act
            this._runner.Run(this._db);

            // Assert
            var migrations = this.GetMigrationHistory();
            migrations.Should().NotBeEmpty();
            migrations[2].Version.ToString().Should().BeEquivalentTo("0.0.3");
        }

        [Fact]
        public void When_database_has_latest_version_Then_nothing_happens()
        {
            // Arrange
            this.InsertMigrations(
                new DatabaseMigration[] { new TestDatabaseMigration_0_0_1(), new TestDatabaseMigration_0_0_2(), new TestDatabaseMigration_0_0_3() });

            // Act
            this._runner.Run(this._db);

            // Assert
            var migrations = this.GetMigrationHistory();
            migrations.Should().NotBeEmpty();
            migrations[0].Version.ToString().Should().BeEquivalentTo("0.0.1");
            migrations[1].Version.ToString().Should().BeEquivalentTo("0.0.2");
            migrations[2].Version.ToString().Should().BeEquivalentTo("0.0.3");
        }
    }
}