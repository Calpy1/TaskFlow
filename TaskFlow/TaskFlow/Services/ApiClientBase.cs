using System.Net.Http;
using System.Windows;

namespace TaskFlow.Services
{
    class ApiClientBase
    {
        public static readonly HttpClient HttpClient = new()
        {
            BaseAddress = new Uri("https://localhost:7035/")
        };

        public static void ShowMessage(string text, string caption, MessageBoxImage icon)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, icon);
        }
    }
}
