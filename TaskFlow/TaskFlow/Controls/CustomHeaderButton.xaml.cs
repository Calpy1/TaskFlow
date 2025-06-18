using System.Windows;
using System.Windows.Controls;

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

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this);
        parentWindow?.Close();
    }
}