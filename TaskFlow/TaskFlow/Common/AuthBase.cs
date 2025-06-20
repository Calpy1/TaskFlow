using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TaskFlow.Controls;
using System.Text.Json;
using System.IO;
using TaskFlow.Models;
using System.Net.NetworkInformation;
using TaskFlow.Views;
using TaskFlow.Services;

namespace TaskFlow.Common
{
    public class AuthBase
    {
        private static readonly SolidColorBrush ErrorBrush = new((Color)ColorConverter.ConvertFromString("#FFE05D5D"));
        private static readonly SolidColorBrush NormalBrush = new((Color)ColorConverter.ConvertFromString("#FF5A6F86"));
        private static readonly SolidColorBrush TransparentBrush = new(Colors.Transparent);
        private string _userMac = GetMac();

        private static readonly HttpClient HttpClient = new()
        {
            BaseAddress = new Uri("https://localhost:7034/")
        };

        private static LoaderView loader = new LoaderView();
        private static LoginView loginView = new LoginView();

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
                    ShowMessage($"Сервер вернул ошибку: {response.StatusCode}", "Ошибка", MessageBoxImage.Warning);
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                ShowMessage($"Ошибка соединения с сервером: {ex.Message}", "Ошибка", MessageBoxImage.Error);
                return false;
            }
        }

        public void AttachInputEventHandlers(CustomTextBox customTextBox)
        {
            var textBox = customTextBox.TextBoxInput;
            textBox.PreviewMouseDown += ClearErrorVisuals;
            textBox.GotFocus += ClearErrorVisuals;
        }

        public bool ValidateRequiredFields(params CustomTextBox[] fields)
        {
            bool allValid = true;
            foreach (var field in fields)
            {
                if (string.IsNullOrWhiteSpace(field.Text))
                {
                    MarkFieldAsError(field);
                    allValid = false;
                }
            }
            return allValid;
        }

        public bool ValidateEmailMatch(CustomTextBox emailField, CustomTextBox confirmEmailField)
        {
            if (!string.IsNullOrWhiteSpace(emailField.Text) &&
                !string.IsNullOrWhiteSpace(confirmEmailField.Text) &&
                !emailField.Text.Equals(confirmEmailField.Text, StringComparison.OrdinalIgnoreCase))
            {
                MarkFieldAsError(emailField);
                MarkFieldAsError(confirmEmailField);
                return false;
            }
            return true;
        }

        public async Task<bool> AuthenticateUserAsync(bool hasValidationErrors, string email, string password, bool rememberMe, string apiEndpoint)
        {
            if (hasValidationErrors)
            {
                return false;
            }

            try
            {
                string hashedPassword = HashPassword(password);
                string? userToken = rememberMe ? Guid.NewGuid().ToString() : null;

                bool isAuthenticated = await AuthenticateWithApiAsync(email, hashedPassword, userToken, _userMac, apiEndpoint);

                if (isAuthenticated)
                {
                    if (apiEndpoint == "api/auth/login" && rememberMe)
                    {
                        SaveUserCredentials(email, userToken);
                    }

                    ShowSuccessMessage(apiEndpoint);
                    return true;
                }
                else
                {
                    ShowMessage("Ошибка входа. Проверьте данные и попробуйте снова.", "Ошибка", MessageBoxImage.Warning);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxImage.Error);
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

                return await AuthenticateWithApiAsync(user.Email, string.Empty, user.Token, _userMac, "api/auth/quicklogin");
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void ClearErrorVisuals(object sender, RoutedEventArgs e) => ResetFieldError(sender);
        private void ClearErrorVisuals(object sender, MouseButtonEventArgs e) => ResetFieldError(sender);

        private void ResetFieldError(object sender)
        {
            if (sender is not TextBox textBox)
            {
                return;
            }

            var parent = FindParentCustomTextBox(textBox);
            if (parent == null)
            {
                return;
            }

            parent.OuterBorder.BorderBrush = TransparentBrush;
            parent.PlaceholderTextBlock.Foreground = NormalBrush;
            parent.PlaceholderSymbolBlock.Foreground = NormalBrush;
        }

        private CustomTextBox? FindParentCustomTextBox(DependencyObject child)
        {
            while (child != null && child is not CustomTextBox)
            {
                child = VisualTreeHelper.GetParent(child);
            }
            return child as CustomTextBox;
        }

        private void MarkFieldAsError(CustomTextBox field)
        {
            field.OuterBorder.BorderBrush = ErrorBrush;
            field.PlaceholderTextBlock.Foreground = ErrorBrush;
            field.PlaceholderSymbolBlock.Foreground = ErrorBrush;
        }

        private static string HashPassword(string password)
        {
            return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password.Trim())));
        }

        private static string GetMac()
        {
            string mac = null;

            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    mac = BitConverter.ToString(networkInterface.GetPhysicalAddress().GetAddressBytes());
                    break;
                }
            }

            return mac;
        }

        private static void SaveUserCredentials(string email, string? token)
        {
            if (token == null)
            {
                return;
            }

            var userData = new { Email = email, Token = token };
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(userData, options);
            File.WriteAllText("user.dat", json);
        }

        private void ShowSuccessMessage(string apiEndpoint)
        {
            string message;

            switch (apiEndpoint)
            {
                case "api/auth/register":
                    //message = "Регистрация прошла успешно.";
                    _ = WindowsService.OpenWindowAsync<LoginView>(FindCurrentWindow());
                    break;
                case "api/auth/login":
                    //message = "Авторизация прошла успешно.";
                    _ = WindowsService.OpenWindowAsync<LoaderView>(FindCurrentWindow());
                    break;
                default:
                    message = string.Empty;
                    break;
            }

            //if (!string.IsNullOrEmpty(message))
            //{
            //    ShowMessage(message, "Успех", MessageBoxImage.Information);
            //}
        }

        private Window? FindCurrentWindow()
        {
            return Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
        }

        private static void ShowMessage(string text, string caption, MessageBoxImage icon)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, icon);
        }
    }
}
