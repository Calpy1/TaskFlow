using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskFlow.Views;

namespace TaskFlow.Services
{
    public class NavigationService
    {
        private static Window? FindCurrentWindow()
        {
            return Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
        }

        public static async void OpenNextWinow(string apiEndpoint)
        {
            string message;

            switch (apiEndpoint)
            {
                case "api/auth/register":
                    //message = "Регистрация прошла успешно.";
                    await WindowsService.OpenWindowAsync<LoginView>(FindCurrentWindow());
                    break;
                case "api/auth/login":
                    //message = "Авторизация прошла успешно.";
                    await WindowsService.OpenWindowAsync<LoaderView>(FindCurrentWindow());
                    await WindowsService.OpenWindowAsync<MainView>(FindCurrentWindow());
                    break;
                case "api/auth/quicklogin":
                    //message = "Авторизация прошла успешно.";
                    await WindowsService.OpenWindowAsync<LoaderView>(FindCurrentWindow());
                    await WindowsService.OpenWindowAsync<MainView>(FindCurrentWindow());
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
    }
}
