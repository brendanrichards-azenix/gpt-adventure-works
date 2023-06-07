using System.Data.SqlClient;
using System.Text.Json;
using Dapper;
using GptAdventureWorks.Web.Data;
using MediatR;

namespace GptAdventureWorks.Web.Application;

public class GetDbColumns: IRequest<string>
{
}

public class GetDbColumnsHandler : IRequestHandler<GetDbColumns, string>
{
    private readonly DbConfig _dbConfig;

    public GetDbColumnsHandler(DbConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<string> Handle(GetDbColumns request, CancellationToken cancellationToken)
    {
        await using var conn = new SqlConnection(_dbConfig.ConnectionString);

        var result =
            conn.QueryAsync(
                "SELECT TABLE_NAME,COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Product'");

        return JsonSerializer.Serialize(result);

    }
}