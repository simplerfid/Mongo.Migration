using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.Documents;
using Mongo.Migration.Services;
using MongoDB.Bson.Serialization;
using Xunit;

namespace Mongo.Migration.Test.MongoDB
{
    
    public class MongoRegistrator_when_registrating : IntegrationBaseTest
    {
        [Fact]
        public void Then_serializer_is_registered()
        {
            // Arrange 
            var migrationService = ServiceProvider.GetRequiredService<IMigrationService>();

            // Act
            migrationService.Migrate();

            // Arrange
            BsonSerializer.LookupSerializer<DocumentVersion>().ValueType.Should().Be(typeof(DocumentVersion));
        }
    }
}