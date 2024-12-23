using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.Documents;
using Mongo.Migration.Services;
using Mongo.Migration.Startup;
using Mongo.Migration.Test.Migrations.Database;
using Xunit;

namespace Mongo.Migration.Test.Services
{
    
    public class DatabaseVersionService_when_determine_version : IntegrationBaseTest
    {

        [Fact]
        public async Task When_project_has_migrations_Then_get_latest_version()
        {
            // Act
            var migrationVersion = ServiceProvider.GetRequiredService<IDatabaseVersionService>().GetCurrentOrLatestMigrationVersion();

            // Assert
            migrationVersion.ToString().Should().Be("0.0.3");
        }

        [Fact]
        public async Task When_version_set_on_startup_Then_use_startup_version()
        {
            // Arrange 

            var settings = ServiceProvider.GetRequiredService<IMongoMigrationSettings>();

            settings.DatabaseMigrationVersion = new DocumentVersion(0, 0, 2);
            
            // Act
            var migrationVersion = ServiceProvider.GetRequiredService<IDatabaseVersionService>().GetCurrentOrLatestMigrationVersion();

            // Assert
            migrationVersion.ToString().Should().Be("0.0.2");
        }
    }
}