using System.Threading.Tasks;
using FluentAssertions;

using Mongo.Migration.Services.Interceptors;
using Mongo.Migration.Test.TestDoubles;
using Xunit;

namespace Mongo.Migration.Test.Services.Interceptors
{
    
    internal class MigrationInterceptorProvider_when_get_serializer : IntegrationTest
    {
        
        public async Task SetUpAsync()
        {
            await this.SetUpAsync();
        }

        
        public async Task TearDownAsync()
        {
            await this.DisposeAsync();
        }

        [Fact]
        public void When_entity_is_document_Then_provide_serializer()
        {
            // Arrange 
            var provider = this._components.Get<IMigrationInterceptorProvider>();

            // Act
            var serializer = provider.GetSerializer(typeof(TestDocumentWithOneMigration));

            // Assert
            serializer.ValueType.Should().Be(typeof(TestDocumentWithOneMigration));
        }

        [Fact]
        public void When_entity_is_not_document_Then_provide_null()
        {
            // Arrange 
            var provider = this._components.Get<IMigrationInterceptorProvider>();

            // Act
            var serializer = provider.GetSerializer(typeof(TestClass));

            // Assert
            serializer.Should().BeNull();
        }
    }
}