using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TaskFlow.Controls;

public partial class CustomTextBox : UserControl
{
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(CustomTextBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty PlaceholderTextProperty =
        DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(CustomTextBox),
            new PropertyMetadata(""));

    public static readonly DependencyProperty PlaceholderSymbProperty =
        DependencyProperty.Register(nameof(PlaceholderSymb), typeof(string), typeof(CustomTextBox),
            new PropertyMetadata(""));

    public CustomTextBox()
    {
        InitializeComponent();

        TextBoxInput.GotFocus += CustomTextBox_GotFocus;
        TextBoxInput.LostFocus += CustomTextBox_LostFocus;
        TextBoxInput.TextChanged += TextBoxInput_TextChanged;

        UpdatePlaceholderVisibility();
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public string PlaceholderSymb
    {
        get => (string)GetValue(PlaceholderSymbProperty);
        set => SetValue(PlaceholderSymbProperty, value);
    }

    private void TextBoxInput_MouseDown(object sender, MouseButtonEventArgs e)
    {
        UpdatePlaceholderVisibility();
    }

    private void CustomTextBox_GotFocus(object? sender, RoutedEventArgs e)
    {
        PlaceholderTextBlock.Visibility = Visibility.Collapsed;
    }

    private void CustomTextBox_LostFocus(object? sender, RoutedEventArgs e)
    {
        UpdatePlaceholderVisibility();
    }

    private void TextBoxInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdatePlaceholderVisibility();
    }

    private void UpdatePlaceholderVisibility()
    {
        if (string.IsNullOrWhiteSpace(TextBoxInput.Text) && !TextBoxInput.IsFocused)
            PlaceholderTextBlock.Visibility = Visibility.Visible;
        else
            PlaceholderTextBlock.Visibility = Visibility.Collapsed;
    }
}