﻿using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.Services.Interceptors;
using Mongo.Migration.Test.TestDoubles;
using Xunit;

namespace Mongo.Migration.Test.Services.Interceptors
{
    
    public class MigrationInterceptorProvider_when_get_serializer : IntegrationBaseTest
    {
        
        [Fact]
        public void When_entity_is_document_Then_provide_serializer()
        {
            // Arrange 
            var provider = ServiceProvider.GetRequiredService<IMigrationInterceptorProvider>();

            // Act
            var serializer = provider.GetSerializer(typeof(TestDocumentWithOneMigration));

            // Assert
            serializer.ValueType.Should().Be(typeof(TestDocumentWithOneMigration));
        }

        [Fact]
        public void When_entity_is_not_document_Then_provide_null()
        {
            // Arrange 
            var provider = ServiceProvider.GetRequiredService<IMigrationInterceptorProvider>();

            // Act
            var serializer = provider.GetSerializer(typeof(TestClass));

            // Assert
            serializer.Should().BeNull();
        }
    }
}