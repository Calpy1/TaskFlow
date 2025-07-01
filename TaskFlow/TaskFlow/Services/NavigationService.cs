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

        public static async Task NavigateToWindowAsync<TWindow>() where TWindow : Window, new()
        {
            var currentWindow = FindCurrentWindow();
            await WindowsService.OpenWindowAsync<LoaderView>(FindCurrentWindow());
            await WindowsService.OpenWindowAsync<TWindow>(FindCurrentWindow());
        }

        public static async Task OpenNextWinowAsync(string apiEndpoint)
        {
            string message;

            switch (apiEndpoint)
            {
                case "api/auth/register":
                    await NavigateToWindowAsync<LoginView>();
                    break;
                case "api/auth/login":
                case "api/auth/quicklogin":
                    await NavigateToWindowAsync<MainView>();
                    break;
                default:
                    message = string.Empty;
                    break;
            }
        }
    }
}
