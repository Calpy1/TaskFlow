﻿using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;
using TaskFlow.Models;
using System.IO;
using TaskFlow.Controls;
using System.Windows.Controls;
using TaskFlow.Views;

namespace TaskFlow.Services
{
    public class AuthService
    {
        private ValidationService _validationService = new ValidationService();
        private static readonly string _userMac = SecurityService.GetMac();

        private static readonly HttpClient HttpClient = new()
        {
            BaseAddress = new Uri("https://localhost:7034/")
        };

        public async Task<bool> AuthenticateWithApiAsync(string email, string hashedPassword, string companySlug, string userToken, string mac, string apiEndpoint)
        {
            var requestData = new { Email = email.Trim(), Password = hashedPassword.Trim(), CompanySlug = companySlug, UserToken = userToken, Mac = mac };

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
            CurrentUser currentUser = new CurrentUser();

            try
            {
                var response = await HttpClient.PostAsJsonAsync(apiEndpoint, requestData);
                if (response.IsSuccessStatusCode)
                {
                    currentUser.Email = email;

                    App.CurrentUser = currentUser;

                    await NavigationService.OpenNextWinowAsync(apiEndpoint);

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

        public async Task<bool> AuthenticateUserAsync(bool hasValidationErrors, string email, string password, string? companySlug, bool rememberMe, string apiEndpoint)
        {
            if (hasValidationErrors)
            {
                return false;
            }

            try
            {
                string hashedPassword = SecurityService.HashPassword(password);
                string? userToken = rememberMe ? Guid.NewGuid().ToString() : null;

                bool isAuthenticated = await AuthenticateWithApiAsync(email, hashedPassword, companySlug, userToken, _userMac, apiEndpoint);

                if (isAuthenticated)
                {
                    if (apiEndpoint == "api/auth/login" && rememberMe)
                    {
                        CredentialsStorageService.SaveUserCredentials(email, userToken);
                    }

                    NavigationService.OpenNextWinowAsync(apiEndpoint);

                    CurrentUser currentUser = new CurrentUser()
                    {
                        Email = email,
                    };

                    App.CurrentUser = currentUser;

                    return true;
                }
                else
                {
                    //ShowMessage("Ошибка входа. Проверьте данные и попробуйте снова.", "Ошибка", MessageBoxImage.Warning); // debug
                    return false;
                }
            }
            catch (Exception ex)
            {
                //ShowMessage($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxImage.Error); // debug
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

        public async Task AttemptLoginAsync(bool validateInputs, CustomTextBox[] fields, CheckBox checkBox)
        {
            var emailTextBox = fields[0];
            var passwordTextBox = fields[1];

            if (validateInputs)
            {
                if (!_validationService.ValidateRequiredFields(emailTextBox, passwordTextBox))
                {
                    return;
                }
            }

            string email = emailTextBox.TextBoxInput.Text;
            string password = passwordTextBox.TextBoxInput.Text;
            bool hasError = string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password);
            bool rememberUser = checkBox.IsChecked == true;

            await AuthenticateUserAsync(hasError, email, password, string.Empty, rememberUser, "api/auth/login");
        }

        public async Task AttemptRegisterAsync(bool validateInputs, CustomTextBox[] fields)
        {
            ValidationService validationService = new ValidationService();
            AuthService authService = new AuthService();

            var emailTextBox = fields[0];
            var passwordTextBox = fields[1];
            var companySlugTextBox = fields[2];

            if (validateInputs)
            {
                if (!validationService.ValidateRequiredFields(emailTextBox, companySlugTextBox, passwordTextBox))
                {
                    return;
                }
            }

            string email = emailTextBox.TextBoxInput.Text;
            string password = passwordTextBox.TextBoxInput.Text;
            string companySlug = companySlugTextBox.TextBoxInput.Text;
            bool hasError = string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(companySlug);

            await authService.AuthenticateUserAsync(hasError, email, password, companySlug, rememberMe: false, "api/auth/register");
        }

        private static void ShowMessage(string text, string caption, MessageBoxImage icon)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, icon);
        }
    }
}
