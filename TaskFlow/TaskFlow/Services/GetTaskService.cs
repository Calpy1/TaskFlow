using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using TaskFlow.Models;
using TaskFlow.Controls;
using Status = TaskFlow.Models.TaskStatus.Status;

namespace TaskFlow.Services
{
    class GetTaskService : ApiClientBase
    {
        public async Task<List<TaskCard>> GetTasksWithApiAsync()
        {
            TaskCardService taskCardService = new TaskCardService();
            List<TaskCard> taskCards = new List<TaskCard>();
            try
            {
                string email = App.CurrentUser.Email;

                var taskList = await HttpClient.GetFromJsonAsync<List<TaskModel>>($"api/tasks/get-tasks?queryEmail={Uri.EscapeDataString(email)}");

                if (taskList.Count > 0)
                {
                    foreach (var task in taskList)
                    {
                        Enum.TryParse<TaskPriority.Priority>(task.TaskPriority, out var taskPriority);
                        Enum.TryParse<Status>(task.TaskStatus, out var taskStatus);

                        var card = await taskCardService.CreateCardAsync(task.TaskName, task.TaskAuthor, task.TaskAssignee, task.DueDate, taskPriority, taskStatus);

                        if (card != null)
                        {
                            taskCards.Add(card);
                        }
                        //MessageBox.Show($"{task.TaskName}\n{task.TaskAuthor}\n{task.TaskAssignee}\n{task.CreatedDate}\n{task.DueDate}\n{task.TaskPriority}\n{task.TaskStatus}"); // debug
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                //ShowMessage($"Ошибка соединения с сервером: {ex.Message}", "Ошибка", MessageBoxImage.Error); // debug
            }
            return taskCards;
        }
    }
}
