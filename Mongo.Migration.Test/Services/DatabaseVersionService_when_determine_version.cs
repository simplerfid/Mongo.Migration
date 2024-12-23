using System.Threading.Tasks;
using FluentAssertions;

using Mongo.Migration.Documents;
using Mongo.Migration.Services;
using Mongo.Migration.Test.Migrations.Database;
using Xunit;

namespace Mongo.Migration.Test.Services
{
    
    internal class DatabaseVersionService_when_determine_version : DatabaseIntegrationTest
    {
        private IDatabaseVersionService _service;

        protected override async Task OnSetUpAsync(DocumentVersion version)
        {
            await base.OnSetUpAsync(version);

            this._service = this._components.Get<IDatabaseVersionService>();
        }

        
        public async Task TearDownAsync()
        {
            await this.DisposeAsync();
        }

        [Fact]
        public async Task When_project_has_migrations_Then_get_latest_version()
        {
            // Arrange 
            await this.OnSetUpAsync(DocumentVersion.Empty());

            // Act
            var migrationVersion = this._service.GetCurrentOrLatestMigrationVersion();

            // Assert
            migrationVersion.ToString().Should().Be("0.0.3");
        }

        [Fact]
        public async Task When_version_set_on_startup_Then_use_startup_version()
        {
            // Arrange 
            await this.OnSetUpAsync(new DocumentVersion(0, 0, 2));

            // Act
            var migrationVersion = this._service.GetCurrentOrLatestMigrationVersion();

            // Assert
            migrationVersion.ToString().Should().Be("0.0.2");
        }
    }
}