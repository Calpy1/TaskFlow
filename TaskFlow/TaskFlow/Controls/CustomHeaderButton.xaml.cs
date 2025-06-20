using System.Windows;
using System.Windows.Controls;
using TaskFlow.Services;

namespace TaskFlow.Controls;

/// <summary>
///     Interaction logic for CustomHeaderButton.xaml
/// </summary>
public partial class CustomHeaderButton : UserControl
{
    public CustomHeaderButton()
    {
        InitializeComponent();
    }

    private async void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this);
        if (parentWindow == null)
        {
            return;
        }

        await WindowsService.FadeOutAsync(parentWindow);
        parentWindow.Close();
    }
}