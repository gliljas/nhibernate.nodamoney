using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TestContainers.Container.Abstractions.Hosting;
using TestContainers.Container.Database.Hosting;
using TestContainers.Container.Database.MsSql;
using Xunit;

namespace NHibernate.NodaMoney.Tests.Fixtures
{

    public class MsSqlContainerFixture : IAsyncLifetime
    {
        public MsSqlContainer Container { get; }

        public string Username { get; } = "sa";

        public string Password { get; } = "Abcd1234!";

        public MsSqlContainerFixture()
        {
            Container = new ContainerBuilder<MsSqlContainer>()
                .ConfigureDockerImageName("mcr.microsoft.com/mssql/server:2017-latest-ubuntu")
                .ConfigureDatabaseConfiguration("not-used", Password, "not-used")
                .ConfigureLogging(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Debug);
                })
                .Build();
        }

        public async Task InitializeAsync()
        {
            await Container.StartAsync();
            using (var connection = new SqlConnection(Container.GetConnectionString("master")))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CREATE DATABASE TestDatabase";
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DisposeAsync()
        {
            await Container.StopAsync();
        }
    }
}
