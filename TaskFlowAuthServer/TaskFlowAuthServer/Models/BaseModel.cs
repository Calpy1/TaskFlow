using MySql.Data.MySqlClient;
using TaskFlowAuthServer.Data;

namespace TaskFlowAuthServer.Models
{
    public class BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        Database database = new Database();

        public async Task<bool> GetScalarValueAsync(string sqlQuery, MySqlParameter[] parameters)
        {
            return await database.ScalarAsync(sqlQuery, parameters);
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
