using System.Data.Common;
using System.Data.SqlClient;
using GptAdventureWorks.Web.Data;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
using Dapper;

namespace GptAdventureWorks.Web.SemanticKernel;

public class SqlServerSkill
{
    private readonly Lazy<Task<SqlConnection>> _lazyConnection;
    
    public SqlServerSkill(DbConfig dbConfig)
    {
        _lazyConnection = new Lazy<Task<SqlConnection>>(async () =>
        {
            var conn = new SqlConnection(dbConfig.ConnectionString);
            await conn.OpenAsync();
            return conn;
        });
    }
    

    [SKFunction("Input is an empty string, output is a comma separated list of tables in the database.")]
    [SKFunctionName("GetSqlTables")]
    public async Task<string> GetSqlTables(string input, SKContext context)
    {
        var conn = await _lazyConnection.Value;
        var tableNames = await conn.QueryAsync<string[]>(@"
SELECT CONCAT(TABLE_SCHEMA, '.', TABLE_NAME)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE='BASE TABLE'
");
        
        return string.Join(",", tableNames);
    }
    
    
    [SKFunction("Input is a single database table name, output is a comma separated list of columns in that table.")]
    [SKFunctionName("GetSqlTables")]
    public async Task<string> GetSqlTableColumns(string input, SKContext context)
    {
        var conn = await _lazyConnection.Value;
        var columnNames = await conn.QueryAsync<string[]>(@"
SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS c
WHERE c.TABLE_NAME='@Product'
", new {Product = input});
        
        return string.Join(",", columnNames);
    }
    
}