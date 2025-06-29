using System.Net.Http;
using System.Windows;
using TaskFlow.Models;
using System.Net.Http.Json;

namespace TaskFlow.Services
{
    class CreateTaskService : ApiClientBase
    {
        public async Task<bool> CreateWithApiAsync(TaskModel task)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync("api/tasks/create", task);
                if (response.IsSuccessStatusCode)
                {
                    //ShowMessage($"Успех: {response.StatusCode}", "Успех", MessageBoxImage.Warning); // debug
                    return true;
                }
                else
                {
                    //ShowMessage($"Сервер вернул ошибку: {response.StatusCode}", "Ошибка", MessageBoxImage.Warning); // debug
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                //ShowMessage($"Ошибка соединения с сервером: {ex.Message}", "Ошибка", MessageBoxImage.Error); // debug
                return false;
            }
        }
    }
}
