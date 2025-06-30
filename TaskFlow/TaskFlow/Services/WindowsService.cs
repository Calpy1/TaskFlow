using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace TaskFlow.Services
{
    public static class WindowsService
    {
        public static async Task<T> OpenWindowAsync<T>(Window currentWindow = null) where T : Window, new()
        {
            if (currentWindow != null)
            {
                await FadeOutAsync(currentWindow);
            }

            var newWindow = new T
            {
                Opacity = 0
            };

            var tcsLoaded = new TaskCompletionSource<bool>();

            void OnLoaded(object sender, RoutedEventArgs e)
            {
                newWindow.Loaded -= OnLoaded;
                tcsLoaded.SetResult(true);
            }

            newWindow.Loaded += OnLoaded;

            newWindow.Show();

            await tcsLoaded.Task;

            await FadeInAsync(newWindow);

            currentWindow?.Close();

            return newWindow;
        }

        public static Task FadeOutAsync(Window window)
        {
            var tcs = new TaskCompletionSource<bool>();

            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(65));
            fadeOut.Completed += (s, e) => tcs.SetResult(true);

            window.BeginAnimation(UIElement.OpacityProperty, fadeOut);

            return tcs.Task;
        }

        public static Task FadeInAsync(Window window)
        {
            var tcs = new TaskCompletionSource<bool>();

            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(65));
            fadeIn.Completed += (s, e) => tcs.SetResult(true);

            window.BeginAnimation(UIElement.OpacityProperty, fadeIn);

            return tcs.Task;
        }
    }
}
