﻿using System;
using System.Threading.Tasks;
using FluentAssertions;

using Mongo.Migration.Services.Interceptors;
using Mongo.Migration.Test.TestDoubles;
using Xunit;

namespace Mongo.Migration.Test.Services.Interceptors
{
    
    internal class MigrationInterceptorFactory_when_creating : IntegrationTest
    {
        [Fact]
        public void If_type_is_assignable_to_document_Then_interceptor_is_created()
        {
            // Arrange
            var factory = this._components.Get<IMigrationInterceptorFactory>();

            // Act
            var interceptor = factory.Create(typeof(TestDocumentWithOneMigration));

            // Assert
            interceptor.ValueType.Should().Be<TestDocumentWithOneMigration>();
        }

        [Fact]
        public void If_type_is_not_assignable_to_document_Then_exception_is_thrown()
        {
            // Arrange
            var factory = this._components.Get<IMigrationInterceptorFactory>();

            // Act
            Action act = () => factory.Create(typeof(TestClass));

            // Assert
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void If_type_is_null_Then_exception_is_thrown()
        {
            // Arrange
            var factory = this._components.Get<IMigrationInterceptorFactory>();

            // Act
            Action act = () => factory.Create(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
        
    }
}