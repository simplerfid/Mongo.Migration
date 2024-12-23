using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;

using Mongo.Migration.Migrations.Document;
using Mongo.Migration.Test.TestDoubles;

using MongoDB.Bson;
using Xunit;

namespace Mongo.Migration.Test.Migrations.Document
{
    
    internal class DocumentMigrationRunner_when_migrating_down : IntegrationTest
    {
        private IDocumentMigrationRunner _runner;

        public DocumentMigrationRunner_when_migrating_down()
        {
            _runner = this._components.Get<IDocumentMigrationRunner>();
        }
        
        [Fact]
        public void When_migrating_down_Then_all_migrations_are_used()
        {
            // Arrange
            BsonDocument document = new BsonDocument
            {
                { "Version", "0.0.2" },
                { "Door", 3 }
            };

            // Act
            this._runner.Run(typeof(TestDocumentWithTwoMigration), document);

            // Assert
            document.Names.ToList()[1].Should().Be("Dors");
            document.Values.ToList()[0].AsString.Should().Be("0.0.0");
        }

        [Fact]
        public void When_document_has_Then_all_migrations_are_used_to_that_version()
        {
            // Arrange
            // Arrange
            BsonDocument document = new BsonDocument
            {
                { "Version", "0.0.2" },
                { "Door", 3 }
            };

            // Act
            this._runner.Run(typeof(TestDocumentWithTwoMigrationMiddleVersion), document);

            // Assert
            document.Names.ToList()[1].Should().Be("Doors");
            document.Values.ToList()[0].AsString.Should().Be("0.0.1");
        }
    }
}