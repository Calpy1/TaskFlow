using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System.Data;

namespace TaskFlowAuthServer.Data
{
    public class Database
    {
        private static readonly string _connectionString = "server=localhost;user=root;password=852741;database=user_data;";

        public async Task<bool> NonQueryAsync(string sqlQuery, MySqlParameter[] parameters)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var mySqlCommand = new MySqlCommand(sqlQuery, conn);
            mySqlCommand.Parameters.AddRange(parameters);

            var result = await mySqlCommand.ExecuteNonQueryAsync();

            return result > 0;
        }

        public async Task<DataTable> QueryAsync(string sqlQuery, MySqlParameter[] parameters)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var mySqlCommand = new MySqlCommand(sqlQuery, conn);
            mySqlCommand.Parameters.AddRange(parameters);

            await using var result = await mySqlCommand.ExecuteReaderAsync();
            var dataTable = new DataTable();
            dataTable.Load(result);
            return dataTable;
        }

        public async Task<bool> ReaderAsync(string sqlQuery, MySqlParameter[] parameters)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var mySqlCommand = new MySqlCommand(sqlQuery, conn);
            mySqlCommand.Parameters.AddRange(parameters);

            await using var result = await mySqlCommand.ExecuteReaderAsync();
            return await result.ReadAsync();
        }

        public async Task<bool> ScalarAsync(string sqlQuery, MySqlParameter[] parameters)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var mySqlCommand = new MySqlCommand(sqlQuery, conn);
            mySqlCommand.Parameters.AddRange(parameters);

            var scalarResult = await mySqlCommand.ExecuteScalarAsync();
            long count = Convert.ToInt64(scalarResult);

            return count > 0;
        }
    }
}
