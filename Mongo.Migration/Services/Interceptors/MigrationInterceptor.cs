using System;
using Mongo.Migration.Documents;
using Mongo.Migration.Migrations.Document;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Mongo.Migration.Services.Interceptors
{
    internal class MigrationInterceptor<TDocument> : IBsonSerializer<TDocument>
        where TDocument : class, IDocument
    {
        private readonly IDocumentVersionService _documentVersionService;

        private readonly IDocumentMigrationRunner _migrationRunner;
        
        private readonly BsonClassMapSerializer<TDocument> _innerSerializer;

        private readonly Type _valueType = typeof(TDocument);

        public MigrationInterceptor(IDocumentMigrationRunner migrationRunner, IDocumentVersionService documentVersionService)
        
        {
            this._migrationRunner = migrationRunner;
            this._documentVersionService = documentVersionService;
            BsonClassMap classMap = BsonClassMap.LookupClassMap(_valueType);
            this._innerSerializer = new BsonClassMapSerializer<TDocument>(classMap);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TDocument value)
        {
            _documentVersionService.DetermineVersion(value);
            _innerSerializer.Serialize(context, args, value);
        }

        public TDocument Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            // TODO: Performance? LatestVersion, dont do anything
            var document = BsonDocumentSerializer.Instance.Deserialize(context);

            this._migrationRunner.Run(_valueType, document);

            var migratedContext =
                BsonDeserializationContext.CreateRoot(new BsonDocumentReader(document));

            return _innerSerializer.Deserialize(migratedContext, args);
        }

        void IBsonSerializer.Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            Serialize(context, args, (TDocument)value);
        }

        public Type ValueType => _valueType;

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }
    }
}