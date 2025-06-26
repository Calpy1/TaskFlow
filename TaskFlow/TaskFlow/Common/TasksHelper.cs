using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;
using TaskFlow.Models;

namespace TaskFlow.Common
{
    public class TasksHelper
    {
        private static readonly HttpClient HttpClient = new()
        {
            BaseAddress = new Uri("https://localhost:7035/")
        };

        public async Task<bool> CreateWithApiAsync(TaskModel task)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync("api/tasks/create", task);
                if (response.IsSuccessStatusCode)
                {
                    ShowMessage($"Успех: {response.StatusCode}", "Успех", MessageBoxImage.Warning); // debug
                    return true;
                }
                else
                {
                    ShowMessage($"Сервер вернул ошибку: {response.StatusCode}", "Ошибка", MessageBoxImage.Warning); // debug
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                ShowMessage($"Ошибка соединения с сервером: {ex.Message}", "Ошибка", MessageBoxImage.Error); // debug
                return false;
            }
        }

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

        private static void ShowMessage(string text, string caption, MessageBoxImage icon)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, icon);
        }
    }
}
