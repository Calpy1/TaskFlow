using MySql.Data.MySqlClient;
using System.Data;
using TaskFlowTaskServer.Data;

namespace TaskFlowTaskServer.Models
{
    public class BaseModel
    {
        public string TaskName { get; set; }
        public string TaskAuthor { get; set; }
        public string AuthorEmail { get; set; }
        public string TaskAssignee { get; set; }
        public string CreatedDate { get; set; }
        public string DueDate { get; set; }
        public string TaskPriority { get; set; }
        public string TaskStatus { get; set; }

        Database database = new Database();

        public async Task<List<Dictionary<string, string>>> GetDataAsync(string sqlQuery)
        {
            var rows = await database.ExecuteQueryAsync(sqlQuery);
            return rows;
        }

        public async Task<bool> AddDataAsync(string sqlQuery, MySqlParameter[] parameters)
        {
            var result = await database.ExecuteNonQueryAsync(sqlQuery, parameters);
            return result;
        }

        public async Task<DataTable> QueryAsync(string sqlQuery, MySqlParameter[] parameters)
        {
            return await database.QueryAsync(sqlQuery, parameters);
        }
    }
}
