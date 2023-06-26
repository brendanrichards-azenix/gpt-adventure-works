using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text.Json;
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
        context.Log.LogInformation("Running GetSqlTables");
        var conn = await _lazyConnection.Value;
        var tableNames = await conn.QueryAsync<string>(@"
SELECT CONCAT(TABLE_SCHEMA, '.', TABLE_NAME)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE='BASE TABLE'
");
        return string.Join(",", tableNames);
    }
    
    
    [SKFunction("Input is a comma separated list of table names. Output is a json document mapping table names to column names")]
    [SKFunctionName("GetSqlTableColumns")]
    public async Task<string> GetSqlTableColumns(string input, SKContext context)
    {
        context.Log.LogInformation("Running GetSqlTableColumns {Input}", input);
        var tablesList = input.Split(",");
        var conn = await _lazyConnection.Value;
        var data = await conn.QueryAsync<GetSqlTableColumnsQueryResult>(@"
SELECT COLUMN_NAME, CONCAT(TABLE_SCHEMA, '.', TABLE_NAME) as Table_Name
FROM INFORMATION_SCHEMA.COLUMNS c
WHERE c.TABLE_NAME in @Tables
", new {Tables = tablesList.Select(t => t.Substring(t.IndexOf('.')+1))});

        return JsonSerializer.Serialize(data);
    }
    

    private class GetSqlTableColumnsQueryResult {
        public string Table_Name { get; set; }
        public string Column_Name { get; set; }
    }

    [SKFunction("Return a random number between 1 and 10")]
    [SKFunctionName("RandomInt1To10")]
    public async Task<string> RandomInt1To10(string input, SKContext context)
    {
        var r = new Random();
        return r.Next(1, 10).ToString();
    }
    
}