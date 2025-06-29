using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;
using TaskFlow.Models;
using System.IO;

namespace TaskFlow.Services
{
    public class AuthService
    {
        private static readonly string _userMac = SecurityService.GetMac();

        private static readonly HttpClient HttpClient = new()
        {
            BaseAddress = new Uri("https://localhost:7034/")
        };

        public async Task<bool> AuthenticateWithApiAsync(string email, string hashedPassword, string userToken, string mac, string apiEndpoint)
        {
            var requestData = new { Email = email.Trim(), Password = hashedPassword.Trim(), UserToken = userToken, Mac = mac };

            try
            {
                var response = await HttpClient.PostAsJsonAsync(apiEndpoint, requestData);
                if (response.IsSuccessStatusCode)
                {
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

        public async Task<bool> QuickLoginWithApiAsync(string email, string userToken, string mac, string apiEndpoint)
        {
            var requestData = new { Email = email.Trim(), UserToken = userToken, Mac = mac };

            try
            {
                var response = await HttpClient.PostAsJsonAsync(apiEndpoint, requestData);
                if (response.IsSuccessStatusCode)
                {
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

        public async Task<bool> AuthenticateUserAsync(bool hasValidationErrors, string email, string password, bool rememberMe, string apiEndpoint)
        {
            if (hasValidationErrors)
            {
                return false;
            }

            try
            {
                string hashedPassword = SecurityService.HashPassword(password);
                string? userToken = rememberMe ? Guid.NewGuid().ToString() : null;

                bool isAuthenticated = await AuthenticateWithApiAsync(email, hashedPassword, userToken, _userMac, apiEndpoint);

                if (isAuthenticated)
                {
                    if (apiEndpoint == "api/auth/login" && rememberMe)
                    {
                        CredentialsStorageService.SaveUserCredentials(email, userToken);
                    }

                    NavigationService.ShowSuccessMessage(apiEndpoint); // debug
                    return true;
                }
                else
                {
                    ShowMessage("Ошибка входа. Проверьте данные и попробуйте снова.", "Ошибка", MessageBoxImage.Warning); // debug
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxImage.Error); // debug
                return false;
            }
        }

        public async Task<bool> TryQuickLoginAsync()
        {
            if (!File.Exists("user.dat"))
            {
                return false;
            }

            try
            {
                var json = File.ReadAllText("user.dat");
                var user = JsonSerializer.Deserialize<UserData>(json);

                if (string.IsNullOrWhiteSpace(user.Email) && string.IsNullOrWhiteSpace(user.Token))
                {
                    return false;
                }
                //MessageBox.Show($"{user.Email}\n{user.Token}\n{GetMac()}");

                return await QuickLoginWithApiAsync(user.Email, user.Token, _userMac, "api/auth/quicklogin");
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static void ShowMessage(string text, string caption, MessageBoxImage icon)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, icon);
        }
    }
}
