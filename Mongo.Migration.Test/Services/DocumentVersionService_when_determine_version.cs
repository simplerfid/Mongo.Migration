using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.Exceptions;
using Mongo.Migration.Services;
using Mongo.Migration.Test.TestDoubles;
using Xunit;

namespace Mongo.Migration.Test.Services
{
    
    public class DocumentVersionService_when_determine_version : IntegrationBaseTest
    {
        [Fact]
        public void When_document_has_current_version_Then_current_version_is_set()
        {
            // Arrange
            var document = new TestDocumentWithTwoMigrationMiddleVersion();

            // Act
            ServiceProvider.GetRequiredService<IDocumentVersionService>().DetermineVersion(document);

            // Assert
            document.Version.Should().Be("0.0.1");
        }

        [Fact]
        public void When_document_has_highest_version_Then_highest_version_is_set()
        {
            // Arrange
            var document = new TestDocumentWithTwoMigrationHighestVersion();

            // Act
            ServiceProvider.GetRequiredService<IDocumentVersionService>().DetermineVersion(document);

            // Assert
            document.Version.Should().Be("0.0.2");
        }

        [Fact]
        public void When_document_has_version_that_should_not_be_Then_throw_exception()
        {
            // Arrange
            var document = new TestDocumentWithTwoMigrationHighestVersion { Version = "0.0.1" };

            // Act// Act
            Action checkAction = () => { ServiceProvider.GetRequiredService<IDocumentVersionService>().DetermineVersion(document); };

            // Assert
            checkAction.Should().Throw<VersionViolationException>();
        }
    }
}