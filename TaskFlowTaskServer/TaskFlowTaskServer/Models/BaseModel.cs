using MySql.Data.MySqlClient;
using TaskFlowTaskServer.Data;

namespace TaskFlowTaskServer.Models
{
    public class BaseModel
    {
        public string TaskName { get; set; }
        public string TaskAuthor { get; set; }
        public string TaskAssignee { get; set; }
        public string CreatedDate { get; set; }
        public string DueDate { get; set; }
        public string TaskPriority { get; set; }
        public string TaskStatus { get; set; }

        Database database = new Database();

        public async Task<List<Dictionary<string, string>>> GetDataAsync(string sqlQuery)
        {
            var rows = await database.ExecuteQuery(sqlQuery);
            return rows;
        }

        public async Task<bool> AddDataAsync(string sqlQuery, MySqlParameter[] parameters)
        {
            var result = await database.ExecuteNonQuery(sqlQuery, parameters);
            return result;
        }
    }
}
