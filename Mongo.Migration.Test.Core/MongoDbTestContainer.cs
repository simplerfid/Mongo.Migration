using System.Threading.Tasks;
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
        await DisposingAsync(_container);
        await _container.DisposeAsync()
            .AsTask();
    }
    
    protected  MongoDbContainer CreateInstance()
    {
        return new MongoDbBuilder()
            .WithUsername(null)
            .WithPassword(null)
            .WithImage("mongo")
            .WithCommand("--replSet", "rs0")
            .Build();
    }
    
    protected virtual Task DisposingAsync(MongoDbContainer container)
    {
        return Task.CompletedTask;
    }
    
    protected async Task ContainerInitializedAsync(MongoDbContainer container)
    {
        ConnectionString = container.GetConnectionString();
        //TODO: This is workaround for MongoDb version 4.4 and should be removed after migrate to version 5
        await container.ExecScriptAsync("rs.initiate({_id:'rs0', members:[{_id:0,host:'127.0.0.1:27017'}]})");
    }
}