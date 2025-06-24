using System.Windows;
using System.Windows.Input;
using TaskFlow.Common;

namespace TaskFlow.Views
{
    /// <summary>
    /// Interaction logic for LoaderView.xaml
    /// </summary>
    public partial class LoaderView : Window
    {
        private WindowPropertiesSaver _windowSaver;
        public LoaderView()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _windowSaver = new WindowPropertiesSaver(this, "LoginFormView", saveFullState: false);

            _windowSaver.Load();

            this.LocationChanged += (s, ev) => _windowSaver.Save();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            _windowSaver?.Save();
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
