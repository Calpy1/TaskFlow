using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskFlow.Common;
using TaskFlow.Controls;
using TaskFlow.Models;
using TaskFlow.Properties;
using TaskFlow.Services;

namespace TaskFlow.Views
{
    public partial class LoginView : Window // TODO: Разделить бизнес-логику и UI по SRP
    {
        private WindowPropertiesSaver _windowSaver;
        private UIErrorService _errorService = new UIErrorService();
        private AuthService _authService = new AuthService();
        public UserData UserData { get; }

        public LoginView()
        {
            InitializeComponent();
            UserData = new UserData();
            DataContext = this;

            _errorService.AttachInputEventHandlers(EmailTextBox);
            _errorService.AttachInputEventHandlers(PasswordTextBox);

            this.MouseDown += Window_MouseDown;

            Loaded += LoginView_Loaded;
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


        private async void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            bool quickLoginSuccess = await _authService.TryQuickLoginAsync();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                LoginViewMainWindow.Focus();
                DragMove();
            }
        }

        private void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            CustomTextBox[] customTextBoxes = new[]
            {
                EmailTextBox,
                PasswordTextBox
            };


            _ = _authService.AttemptLoginAsync(validateInputs: true, customTextBoxes, CheckBoxRememberMe);
        }

        private void HaveAccountButton_Click(object sender, RoutedEventArgs e)
        {
            _ = WindowsService.OpenWindowAsync<RegisterView>(this);
        }
    }
}