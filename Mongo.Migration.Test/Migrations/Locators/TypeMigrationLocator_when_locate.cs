using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;

using Mongo.Migration.Migrations.Locators;
using Mongo.Migration.Test.TestDoubles;
using Xunit;

namespace Mongo.Migration.Test.Migrations.Locators
{
    
    public class TypeMigrationLocator_when_locate
    {
        private TypeMigrationLocator _locator;

        public TypeMigrationLocator_when_locate()
        {
            this._locator = new TypeMigrationLocator();
        }
        
        [Fact]
        public void When_document_has_one_migration_Then_migrations_count_should_be_one()
        {
            // Act
            var result = this._locator.GetMigrations(typeof(TestDocumentWithOneMigration));

            // Assert
            result.Count().Should().Be(1);
        }

        [Fact]
        public void When_document_has_two_migration_Then_migrations_count_should_be_two()
        {
            // Act
            var result = this._locator.GetMigrations(typeof(TestDocumentWithTwoMigration));

            // Assert
            result.Count().Should().Be(2);
        }

        [Fact]
        public void When_get_latest_version_of_migrations()
        {
            // Act
            var version = this._locator.GetLatestVersion(typeof(TestDocumentWithTwoMigration));

            // Assert
            version.Should().Be("0.0.2");
        }

        [Fact]
        public void When_get_migrations_gt_and_equal_version()
        {
            // Act
            var result = this._locator.GetMigrationsGtEq(typeof(TestDocumentWithTwoMigration), "0.0.1").ToList();

            // Assert
            result[0].Should().BeOfType<TestDocumentWithTwoMigration_0_0_1>();
            result[1].Should().BeOfType<TestDocumentWithTwoMigration_0_0_2>();
        }

        [Fact]
        public void When_get_migrations_gt_version()
        {
            // Act
            var result = this._locator.GetMigrationsGt(typeof(TestDocumentWithTwoMigration), "0.0.1").ToList();

            // Assert
            result[0].Should().BeOfType<TestDocumentWithTwoMigration_0_0_2>();
        }
    }
}