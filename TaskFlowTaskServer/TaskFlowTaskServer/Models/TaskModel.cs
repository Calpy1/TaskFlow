using MySql.Data;
using MySql.Data.MySqlClient;

namespace TaskFlowTaskServer.Models
{
    public class TaskModel : BaseModel
    {
        public async Task<List<BaseModel>> GetTasksAsync()
        {
            var rows = await GetDataAsync("SELECT * FROM task_data.tasks");

            List<BaseModel> tasks = new List<BaseModel>();

            foreach (var row in rows)
            {
                BaseModel taskModel = new BaseModel();

                foreach (var pair in row)
                {
                    string key = pair.Key;
                    string value = pair.Value;

                    switch (key)
                    {
                        case "task_name":
                            taskModel.TaskName = value;
                            break;
                        case "task_author":
                            taskModel.TaskAuthor = value;
                            break;
                        case "task_assignee":
                            taskModel.TaskAssignee = value;
                            break;
                        case "created_date":
                            taskModel.CreatedDate = value;
                            break;
                        case "due_date":
                            taskModel.DueDate = value;
                            break;
                        case "task_priority":
                            taskModel.TaskPriority = value;
                            break;
                        case "task_status":
                            taskModel.TaskStatus = value;
                            break;
                    }
                }

                tasks.Add(taskModel);
            }

            return tasks;
        }

        public async Task<bool> AddTaskAsync()
        {
            try
            {
                string query = "INSERT INTO task_data.tasks (task_name, task_author, task_assignee, created_date, due_date, task_priority, task_status) VALUES (@TaskName, @TaskAuthor, @TaskAssignee, @CreatedDate, @DueDate, @TaskPriority, @TaskStatus)";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@TaskName", this.TaskName),
                    new MySqlParameter("@TaskAuthor", this.TaskAuthor),
                    new MySqlParameter("@TaskAssignee", this.TaskAssignee),
                    new MySqlParameter("@CreatedDate", this.CreatedDate),
                    new MySqlParameter("@DueDate", this.DueDate),
                    new MySqlParameter("@TaskPriority", this.TaskPriority),
                    new MySqlParameter("@TaskStatus", this.TaskStatus),
                };
                return await AddDataAsync(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
