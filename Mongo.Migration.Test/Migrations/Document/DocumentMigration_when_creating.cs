using FluentAssertions;

using Mongo.Migration.Test.TestDoubles;
using Xunit;

namespace Mongo.Migration.Test.Migrations.Document
{
    
    public class DocumentMigration_when_creating
    {
        [Fact]
        public void Then_migration_has_type_testClass()
        {
            // Arrange Act
            var migration = new TestDocumentWithOneMigration_0_0_1();

            // Assert
            migration.Type.Should().Be(typeof(TestDocumentWithOneMigration));
        }

        [Fact]
        public void Then_migration_have_version()
        {
            // Arrange Act
            var migration = new TestDocumentWithOneMigration_0_0_1();

            // Assert
            migration.Version.Should().Be("0.0.1");
        }

        [Fact]
        public void Then_migration_should_be_created()
        {
            // Arrange Act
            var migration = new TestDocumentWithOneMigration_0_0_1();

            // Assert
            migration.Should().BeOfType<TestDocumentWithOneMigration_0_0_1>();
        }
    }
}