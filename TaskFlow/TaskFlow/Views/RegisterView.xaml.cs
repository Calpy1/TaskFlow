using System.Windows;
using System.Windows.Input;
using TaskFlow.Common;
using TaskFlow.Models;

namespace TaskFlow.Views
{
    public partial class RegisterView : Window
    {
        public UserData UserData { get; }
        private readonly AuthBase _auth;

        public RegisterView()
        {
            InitializeComponent();
            UserData = new UserData();
            DataContext = this;
            _auth = new AuthBase();

            _auth.AttachInputEventHandlers(EmailTextBox);
            _auth.AttachInputEventHandlers(ConfirmEmailTextBox);
            _auth.AttachInputEventHandlers(PasswordTextBox);

            this.MouseDown += Window_MouseDown;
            this.KeyDown += Window_KeyDown;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.Focus();
                DragMove();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
                AttemptRegister(validateInputs: false);
            }
        }

        private void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            AttemptRegister(validateInputs: true);
        }

        private void AttemptRegister(bool validateInputs)
        {
            if (validateInputs)
            {
                if (!_auth.ValidateRequiredFields(EmailTextBox, ConfirmEmailTextBox, PasswordTextBox))
                {
                    return;
                }

                if (!_auth.ValidateEmailMatch(EmailTextBox, ConfirmEmailTextBox))
                {
                    return;
                }
            }

            string email = EmailTextBox.TextBoxInput.Text;
            string password = PasswordTextBox.TextBoxInput.Text;
            bool hasError = string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password);

            _ = _auth.AuthenticateUserAsync(hasError, email, password, rememberMe: false, "api/auth/register");
        }

        private void HaveAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginView();
            loginWindow.Show();
            this.Close();
        }
    }
}
