using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace TaskFlow.Services
{
    public static class WindowsService
    {
        public static async Task OpenWindowAsync<T>(Window currentWindow = null) where T : Window, new()
        {
            if (currentWindow != null)
            {
                await FadeOutAsync(currentWindow);
            }

            var newWindow = new T
            {
                Opacity = 0
            };

            newWindow.Show();
            await FadeInAsync(newWindow);

            currentWindow?.Close();
        }

        public static Task FadeOutAsync(Window window)
        {
            var tcs = new TaskCompletionSource<bool>();

            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(60));
            fadeOut.Completed += (s, e) => tcs.SetResult(true);

            window.BeginAnimation(UIElement.OpacityProperty, fadeOut);

            return tcs.Task;
        }

        public static Task FadeInAsync(Window window)
        {
            var tcs = new TaskCompletionSource<bool>();

            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(60));
            fadeIn.Completed += (s, e) => tcs.SetResult(true);

            window.BeginAnimation(UIElement.OpacityProperty, fadeIn);

            return tcs.Task;
        }
    }
}
