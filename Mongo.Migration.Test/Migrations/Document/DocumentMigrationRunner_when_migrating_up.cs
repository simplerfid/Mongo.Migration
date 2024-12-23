using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;

using Mongo.Migration.Migrations.Document;
using Mongo.Migration.Test.TestDoubles;

using MongoDB.Bson;
using Xunit;

namespace Mongo.Migration.Test.Migrations.Document
{
    
    internal class DocumentMigrationRunner_when_migrating_up : IntegrationTest
    {
        private IDocumentMigrationRunner _runner;

        public DocumentMigrationRunner_when_migrating_up()
        {
            _runner = _components.Get<IDocumentMigrationRunner>();
        }
        
        
        [Fact]
        public void When_migrate_up_the_lowest_version_Then_all_migrations_are_used()
        {
            // Arrange
            BsonDocument document = new BsonDocument
            {
                { "Version", "0.0.0" },
                { "Dors", 3 }
            };

            // Act
            this._runner.Run(typeof(TestDocumentWithTwoMigrationHighestVersion), document);

            // Assert
            document.Names.ToList()[1].Should().Be("Door");
            document.Values.ToList()[0].AsString.Should().Be("0.0.2");
        }

        [Fact]
        public void When_document_has_no_version_Then_all_migrations_are_used()
        {
            // Arrange
            BsonDocument document = new BsonDocument
            {
                { "Dors", 3 }
            };

            // Act
            this._runner.Run(typeof(TestDocumentWithTwoMigrationHighestVersion), document);

            // Assert
            document.Names.ToList()[1].Should().Be("Door");
            document.Values.ToList()[0].AsString.Should().Be("0.0.2");
        }

        [Fact]
        public void When_document_has_current_version_Then_nothing_happens()
        {
            // Arrange
            BsonDocument document = new BsonDocument
            {
                { "Version", "0.0.2" },
                { "Door", 3 }
            };

            // Act
            this._runner.Run(typeof(TestDocumentWithTwoMigrationHighestVersion), document);

            // Assert
            document.Names.ToList()[1].Should().Be("Door");
            document.Values.ToList()[0].AsString.Should().Be("0.0.2");
        }
    }
}