using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.Services.Interceptors;
using Mongo.Migration.Test.TestDoubles;
using Xunit;

namespace Mongo.Migration.Test.Services.Interceptors
{
    
    public class MigrationInterceptorFactory_when_creating : IntegrationBaseTest
    {
        [Fact]
        public void If_type_is_assignable_to_document_Then_interceptor_is_created()
        {
            // Arrange
            var factory = ServiceProvider.GetRequiredService<IMigrationInterceptorFactory>();

            // Act
            var interceptor = factory.Create(typeof(TestDocumentWithOneMigration));

            // Assert
            interceptor.ValueType.Should().Be<TestDocumentWithOneMigration>();
        }

        [Fact]
        public void If_type_is_not_assignable_to_document_Then_exception_is_thrown()
        {
            // Arrange
            var factory = ServiceProvider.GetRequiredService<IMigrationInterceptorFactory>();

            // Act
            Action act = () => factory.Create(typeof(TestClass));

            // Assert
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void If_type_is_null_Then_exception_is_thrown()
        {
            // Arrange
            var factory = ServiceProvider.GetRequiredService<IMigrationInterceptorFactory>();

            // Act
            Action act = () => factory.Create(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
        
    }
}