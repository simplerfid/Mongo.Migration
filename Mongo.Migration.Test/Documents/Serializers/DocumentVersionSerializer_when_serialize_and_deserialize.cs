using System.Threading.Tasks;
using FluentAssertions;

using Mongo.Migration.Documents;
using Mongo.Migration.Documents.Serializers;

using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Xunit;

namespace Mongo.Migration.Test.Documents.Serializers
{
    
    public class DocumentVersionSerializer_when_serialize_and_deserialize
    {
        private DocumentVersionSerializer _serializer;

        public DocumentVersionSerializer_when_serialize_and_deserialize()
        {
            this._serializer = new DocumentVersionSerializer();
        }
        
        [Fact]
        public void Then_version_is_deserialized_correct()
        {
            // Arrange 
            var document = new BsonDocument { { "version", "0.1.1" } };
            BsonDocumentReader reader = CreateVersionReader(document);

            BsonDeserializationContext context = BsonDeserializationContext.CreateRoot(reader);
            var args = new BsonDeserializationArgs { NominalType = typeof(DocumentVersion) };

            // Act 
            DocumentVersion result = this._serializer.Deserialize(context, args);

            // Assert 
            result.Should().BeOfType<DocumentVersion>();
            result.Should().Be("0.1.1");
        }

        [Fact]
        public void Then_version_is_serialized_correct()
        {
            // Arrange 
            BsonDocumentWriter writer = CreateVersionWriter();
            BsonSerializationContext context = BsonSerializationContext.CreateRoot(writer);
            var args = new BsonSerializationArgs { NominalType = typeof(DocumentVersion) };
            var version = new DocumentVersion("0.0.1");

            // Act 
            this._serializer.Serialize(context, args, version);

            // Assert 
            BsonDocument document = writer.Document;
            document.ToString().Should().Be("{ \"version\" : \"0.0.1\" }");
        }

        
        public void SetUp()
        {
            
        }

        private static BsonDocumentReader CreateVersionReader(BsonDocument document)
        {
            var reader = new BsonDocumentReader(document);
            reader.ReadStartDocument();
            reader.ReadName();
            return reader;
        }

        private static BsonDocumentWriter CreateVersionWriter()
        {
            var writer = new BsonDocumentWriter(new BsonDocument());
            writer.WriteStartDocument();
            writer.WriteName("version");
            return writer;
        }
    }
}