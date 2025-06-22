using System.Windows;
using System.Windows.Input;

namespace TaskFlow.Views
{
    /// <summary>
    /// Interaction logic for LoaderView.xaml
    /// </summary>
    public partial class LoaderView : Window
    {
        public LoaderView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
