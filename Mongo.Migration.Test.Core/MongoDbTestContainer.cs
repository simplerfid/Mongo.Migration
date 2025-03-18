using System.Threading.Tasks;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using Xunit;

namespace Mongo.Migration.Test.Core;

public class MongoDbTestContainer : IAsyncLifetime
{
    private MongoDbContainer _container;
    
    public string ConnectionString { get; private set; }
    

    public async Task InitializeAsync()
    {
        _container = CreateInstance();
        await _container.StartAsync();
        await ContainerInitializedAsync(_container);
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync()
            .AsTask();
    }
    
    protected  MongoDbContainer CreateInstance()
    {
        return new MongoDbBuilder()
            .WithImage("mongo")
            .Build();
    }

    protected async Task ContainerInitializedAsync(MongoDbContainer container)
    {
        //TODO: This workaround for fix issue https://jira.mongodb.org/projects/CSHARP/issues/CSHARP-5445?filter=allissues
        ConnectionString = new MongoUrlBuilder(container.GetConnectionString())
        {
            DirectConnection = true,
        }.ToMongoUrl().ToString();
    }
}