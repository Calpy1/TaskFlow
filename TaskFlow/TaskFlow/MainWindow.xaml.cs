using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace TaskFlow;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private static readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7034/")
    };

    public MainWindow()
    {
        InitializeComponent();
    }
}