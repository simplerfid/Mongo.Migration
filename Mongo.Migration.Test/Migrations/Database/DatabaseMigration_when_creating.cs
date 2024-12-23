using FluentAssertions;

using Mongo.Migration.Migrations.Database;
using Mongo.Migration.Test.TestDoubles;
using Xunit;

namespace Mongo.Migration.Test.Migrations.Database
{
    
    public class DatabaseMigration_when_creating
    {
        [Fact]
        public void Then_migration_has_type_DatabaseMigration()
        {
            // Arrange Act
            var migration = new TestDatabaseMigration_0_0_1();

            // Assert
            migration.Type.Should().Be(typeof(DatabaseMigration));
        }

        [Fact]
        public void Then_migration_have_version()
        {
            // Arrange Act
            var migration = new TestDatabaseMigration_0_0_1();

            // Assert
            migration.Version.Should().Be("0.0.1");
        }

        [Fact]
        public void Then_migration_should_be_created()
        {
            // Arrange Act
            var migration = new TestDatabaseMigration_0_0_1();

            // Assert
            migration.Should().BeOfType<TestDatabaseMigration_0_0_1>();
        }
    }
}