using System;
using FluentAssertions;

using Mongo.Migration.Exceptions;
using Mongo.Migration.Startup.Static;
using NSubstitute;
using Xunit;

namespace Mongo.Migration.Test.Services.Initializers
{
    
    public class MongoMigration_when_initialize
    {
        
        [Fact]
        public void When_inizialize_twice_Then_throw_exception()
        {
            // Arrange
            var registry = Substitute.For<IComponentRegistry>();
            var mongoMigration = Substitute.For<IMongoMigration>();

            registry.Get<IMongoMigration>().Returns(mongoMigration);

            // Act
            MongoMigrationClient.Initialize(registry);

            Action comparison = () => MongoMigrationClient.Initialize(registry);

            // Assert
            comparison.Should().Throw<AlreadyInitializedException>();
        }
    }
}