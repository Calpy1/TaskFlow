using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    class GetTaskService : ApiClientBase
    {
        public async Task<List<TaskModel>> GetTaskWithApiAsync()
        {
            List<TaskModel> taskList = new List<TaskModel>();
            try
            {
                taskList = await HttpClient.GetFromJsonAsync<List<TaskModel>>("api/tasks/get-tasks");

                if (taskList.Count > 0)
                {
                    foreach (var task in taskList)
                    {
                        MessageBox.Show($"{task.TaskName}\n{task.TaskAuthor}\n{task.TaskAssignee}\n{task.CreatedDate}\n{task.DueDate}\n{task.TaskPriority}\n{task.TaskStatus}");
                    }

                    return taskList;
                }
                else
                {
                    MessageBox.Show("Сервер вернул пустой список задач.");
                    return new List<TaskModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                ShowMessage($"Ошибка соединения с сервером: {ex.Message}", "Ошибка", MessageBoxImage.Error); // debug
                return null;
            }
        }
    }
}
