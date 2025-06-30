using MySql.Data.MySqlClient;
using System.Data;
using TaskFlowAuthServer.Data;

namespace TaskFlowAuthServer.Models
{
    public class BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanySlug { get; set; }

        Database database = new Database();

        public async Task<bool> GetScalarValueAsync(string sqlQuery, MySqlParameter[] parameters)
        {
            return await database.ScalarAsync(sqlQuery, parameters);
        }

        public async Task<DataTable> ExecuteQueryAsync(string sqlQuery, MySqlParameter[] parameters)
        {
            return await database.QueryAsync(sqlQuery, parameters);
        }

        public async Task<bool> ExecuteNonQueryAsync(string sqlQuery, MySqlParameter[] parameters)
        {
            return await database.NonQueryAsync(sqlQuery, parameters);
        }

        public async Task<bool> ExecuteReader(string sqlQuery, MySqlParameter[] parameters)
        {
            return await database.ReaderAsync(sqlQuery, parameters);
        }
    }
}
