using FluentAssertions;

using Mongo.Migration.Documents;
using Xunit;

namespace Mongo.Migration.Test.Documents
{
    
    public class Document_when_creating
    {
        [Fact]
        public void Then_document_can_be_created()
        {
            // Arrange Act
            IDocument document = new Document();

            // Assert
            document.Should().BeOfType<Document>();
        }

        [Fact]
        public void Then_document_has_a_version()
        {
            // Arrange 
            IDocument document = new Document();

            // Act
            var version = document.Version;

            // Assert
            version.Should().Be("0.0.0");
        }
    }
}