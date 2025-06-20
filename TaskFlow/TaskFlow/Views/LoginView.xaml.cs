using System.Windows;
using System.Windows.Input;
using TaskFlow.Common;
using TaskFlow.Models;
using TaskFlow.Services;

namespace TaskFlow.Views
{
    public partial class LoginView : Window
    {
        public UserData UserData { get; }
        private readonly AuthBase _auth;

        public LoginView()
        {
            InitializeComponent();
            UserData = new UserData();
            DataContext = this;
            _auth = new AuthBase();

            _auth.AttachInputEventHandlers(EmailTextBox);
            _auth.AttachInputEventHandlers(PasswordTextBox);

            this.MouseDown += Window_MouseDown;
            //this.KeyDown += Window_KeyDown;

            Loaded += LoginView_Loaded;
        }

        private async void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            bool quickLoginSuccess = await _auth.TryQuickLoginAsync();

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

        private void AttemptLogin(bool validateInputs)
        {
            if (validateInputs)
            {
                if (!_auth.ValidateRequiredFields(EmailTextBox, PasswordTextBox))
                {
                    return;
                }
            }

            string email = EmailTextBox.TextBoxInput.Text;
            string password = PasswordTextBox.TextBoxInput.Text;
            bool hasError = string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password);
            bool rememberUser = CheckBoxRememberMe.IsChecked == true;

            _ = _auth.AuthenticateUserAsync(hasError, email, password, rememberUser, "api/auth/login");
        }

        private void HaveAccountButton_Click(object sender, RoutedEventArgs e)
        {
            _ = WindowsService.OpenWindowAsync<RegisterView>(this);
        }
    }
}
