using System.Windows;
using System.Windows.Input;
using TaskFlow.Common;
using TaskFlow.Models;
using TaskFlow.Properties;
using TaskFlow.Services;

namespace TaskFlow.Views
{
    public partial class RegisterView : Window
    {
        private WindowPropertiesSaver _windowSaver;
        public UserData UserData { get; }
        private readonly AuthHelper _auth;

        public RegisterView()
        {
            InitializeComponent();
            UserData = new UserData();
            DataContext = this;
            _auth = new AuthHelper();

            _auth.AttachInputEventHandlers(EmailTextBox);
            _auth.AttachInputEventHandlers(ConfirmEmailTextBox);
            _auth.AttachInputEventHandlers(PasswordTextBox);

            this.MouseDown += Window_MouseDown;
            //this.KeyDown += Window_KeyDown;
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
                RegisterMainViewWindow.Focus();
                DragMove();
            }
        }

        private void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            AttemptRegister(validateInputs: true);
        }

        private async void AttemptRegister(bool validateInputs)
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

            await _auth.AuthenticateUserAsync(hasError, email, password, rememberMe: false, "api/auth/register");
        }

        private void HaveAccountButton_Click(object sender, RoutedEventArgs e)
        {
            _ = WindowsService.OpenWindowAsync<LoginView>(this);
        }
    }
}
