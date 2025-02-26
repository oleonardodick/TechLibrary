using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Respawn;
using System.Data.Common;
using TechLibrary.Infrastructure.DataAccess;
using Testcontainers.PostgreSql;

namespace TechLibrary.Tests.IntegrationTests
{
    public class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private PostgreSqlContainer _dbContainer;
        public HttpClient Client { get; private set; } = default!;
        private DbConnection _dbConnection = default!;
        private Respawner _respawner = default!;

        public IntegrationTestFactory()
        {
            _dbContainer = new PostgreSqlBuilder().Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var connectionString = _dbContainer.GetConnectionString();
            base.ConfigureWebHost(builder);
            builder.ConfigureTestServices(service =>
            {
                service.RemoveAll(typeof(DbContextOptions<TechLibraryDbContext>));
                service.AddDbContext<TechLibraryDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });
            });
        }

        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_dbConnection);
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
            Client = CreateClient();

            using(var scope = Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<TechLibraryDbContext>();

                await cntx.Database.EnsureCreatedAsync();
            }
            await InitializeRespawner();
        }

        private async Task InitializeRespawner()
        {
            await _dbConnection.OpenAsync();
            _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[] { "public" }
            });
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }
    }
}
