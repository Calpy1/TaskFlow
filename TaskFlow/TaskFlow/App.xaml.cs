using System.Windows;
using TaskFlow.Models;

namespace TaskFlow;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static CurrentUser CurrentUser { get; set; }
}