using System.Windows;
using System.Windows.Input;
using TaskFlow.Common;
using TaskFlow.Models;
using TaskFlow.Properties;
using TaskFlow.Services;

namespace TaskFlow.Views
{
    public partial class LoginView : Window
    {
        private WindowPropertiesSaver _windowSaver;
        private UIErrorService _errorService = new UIErrorService();
        private AuthService _authService = new AuthService();
        private ValidationService _validationService = new ValidationService();
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

            if (quickLoginSuccess)
            {
                //MessageBox.Show("Быстрый вход успешен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                // TODO: перейти к главному окну

                await WindowsService.OpenWindowAsync<LoaderView>(this);
            }
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
            AttemptLogin(validateInputs: true);
        }

        private async void AttemptLogin(bool validateInputs)
        {
            if (validateInputs)
            {
                if (!_validationService.ValidateRequiredFields(EmailTextBox, PasswordTextBox))
                {
                    return;
                }
            }

            string email = EmailTextBox.TextBoxInput.Text;
            string password = PasswordTextBox.TextBoxInput.Text;
            bool hasError = string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password);
            bool rememberUser = CheckBoxRememberMe.IsChecked == true;

            await _authService.AuthenticateUserAsync(hasError, email, password, rememberUser, "api/auth/login");
        }

        private void HaveAccountButton_Click(object sender, RoutedEventArgs e)
        {
            _ = WindowsService.OpenWindowAsync<RegisterView>(this);
        }
    }
}
