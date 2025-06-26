using TaskFlowTaskServer.Models;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace TaskFlowTaskServer.Data
{
    public class Database
    {
        private readonly string _connectionString = "server=localhost;user=root;password=852741;database=task_data;";

        public async Task<bool> ExecuteNonQuery(string sqlQuery, MySqlParameter[] parameters)
        {
            await using MySqlConnection conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            using var mySqlCommand = new MySqlCommand(sqlQuery, conn);
            mySqlCommand.Parameters.AddRange(parameters);

            int rowsAffected = await mySqlCommand.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<List<Dictionary<string, string>>> ExecuteQuery(string rawSql)
        {
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            await using MySqlConnection conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            using var mySqlCommand = new MySqlCommand(rawSql, conn);
            var reader = await mySqlCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(reader.GetName(i), reader.GetValue(i).ToString());
                }
                rows.Add(row);
            }

            return rows;
        }
    }
}
