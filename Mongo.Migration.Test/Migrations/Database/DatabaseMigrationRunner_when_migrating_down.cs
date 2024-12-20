using System.Threading.Tasks;
using FluentAssertions;

using Mongo.Migration.Documents;
using Mongo.Migration.Migrations.Database;
using Mongo.Migration.Test.TestDoubles;
using Xunit;

namespace Mongo.Migration.Test.Migrations.Database
{
    
    internal class DatabaseMigrationRunner_when_migrating_down : DatabaseIntegrationTest
    {
        private IDatabaseMigrationRunner _runner;

        protected override async Task OnSetUpAsync(DocumentVersion databaseMigrationVersion)
        {
            await base.OnSetUpAsync(databaseMigrationVersion);

            this._runner = this._components.Get<IDatabaseMigrationRunner>();
        }

        
        public async Task TearDownAsync()
        {
            await DisposeAsync();
        }

        [Fact]
        public async Task When_database_has_migrations_Then_down_all_migrations()
        {
            await this.OnSetUpAsync(DocumentVersion.Default());

            // Arrange
            this.InsertMigrations(
                new DatabaseMigration[]
                {
                    new TestDatabaseMigration_0_0_1(),
                    new TestDatabaseMigration_0_0_2(),
                    new TestDatabaseMigration_0_0_3()
                });

            // Act
            this._runner.Run(this._db);

            // Assert
            var migrations = this.GetMigrationHistory();
            migrations.Should().BeEmpty();
        }

        [Fact]
        public async Task When_database_has_migrations_Then_down_to_selected_migration()
        {
            await this.OnSetUpAsync(new DocumentVersion("0.0.1"));

            // Arrange
            this.InsertMigrations(
                new DatabaseMigration[]
                {
                    new TestDatabaseMigration_0_0_1(),
                    new TestDatabaseMigration_0_0_2(),
                    new TestDatabaseMigration_0_0_3()
                });

            // Act
            this._runner.Run(this._db);

            // Assert
            var migrations = this.GetMigrationHistory();
            migrations.Should().NotBeEmpty();
            migrations.Should().OnlyContain(m => m.Version == "0.0.1");
        }
    }
}