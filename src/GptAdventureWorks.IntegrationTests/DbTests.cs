using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace GptAdventureWorks.IntegrationTests;

public class DbTests
{
    [Fact]
    public async Task ShouldQueryDatabase()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .Build();

        var connectionString = config["DbConnectionString"];
        connectionString.Should().NotBeEmpty("Expect connection string to be configured with name DbConnectionString");
        
        await using var conn = new SqlConnection(connectionString);
        await conn.OpenAsync();
        var count = await conn.ExecuteScalarAsync("select count(ProductId) from [Production].[Product]");
        count.Should().NotBe(0);
    }
}