using System.Windows;
using System.Windows.Input;
using TaskFlow.Common;
using TaskFlow.Controls;
using TaskFlow.Models;
using TaskFlow.Properties;
using TaskFlow.Services;

namespace TaskFlow.Views
{
    public partial class RegisterView : Window
    {
        private WindowPropertiesSaver _windowSaver;
        UIErrorService _errorService;
        AuthService _authService = new AuthService();
        public UserData UserData { get; }

        public RegisterView()
        {
            InitializeComponent();
            UserData = new UserData();
            DataContext = this;

            _errorService = new UIErrorService(EmailTextBox, PasswordTextBox, CompanySlug);

            this.MouseDown += Window_MouseDown;
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
                this.Focus();
                DragMove();
            }
        }

        private async void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            CustomTextBox[] customTextBoxes = new[]
            {
                EmailTextBox,
                PasswordTextBox,
                CompanySlug,
            };

            await _authService.AttemptRegisterAsync(validateInputs: true, customTextBoxes);
        }

        private void HaveAccountButton_Click(object sender, RoutedEventArgs e)
        {
            _ = WindowsService.OpenWindowAsync<LoginView>(this);
        }
    }
}
