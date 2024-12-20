using System.Threading.Tasks;
using FluentAssertions;

using Mongo.Migration.Documents;
using Mongo.Migration.Services;
using MongoDB.Bson.Serialization;
using Xunit;

namespace Mongo.Migration.Test.MongoDB
{
    
    internal class MongoRegistrator_when_registrating : IntegrationTest
    {
        [Fact]
        public void Then_serializer_is_registered()
        {
            // Arrange 
            var migrationService = this._components.Get<IMigrationService>();

            // Act
            migrationService.Migrate();

            // Arrange
            BsonSerializer.LookupSerializer<DocumentVersion>().ValueType.Should().Be(typeof(DocumentVersion));
        }
    }
}