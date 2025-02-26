using Microsoft.Extensions.DependencyInjection;
using TechLibrary.Infrastructure.DataAccess;

namespace TechLibrary.Tests.IntegrationTests
{
    public abstract class IntegrationTestBase : IClassFixture<IntegrationTestFactory>, IAsyncLifetime
    {
        protected readonly IntegrationTestFactory _factory;
        protected readonly HttpClient _client;
        protected readonly TechLibraryDbContext _dbContext = default!;
        private readonly IServiceScope _scope;

        public IntegrationTestBase(IntegrationTestFactory factory)
        {
            _factory = factory;
            _client = _factory.Client;
            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<TechLibraryDbContext>();
        }

        public async Task DisposeAsync() {
            await _factory.ResetDatabaseAsync();
            _scope.Dispose();
        }

        public Task InitializeAsync() => Task.CompletedTask;
    }
}
