using System;
using System.Windows.Controls;

using WAF_exercise_Library_Portal_1_Core_WPF.Models;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ILibraryModel _model;

        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand LoginCommand { get; private set; }

        public String UserName { get; set; }

        public event EventHandler ExitApplication;
        public event EventHandler LoginSuccess;
        public event EventHandler LoginFailed;

        public LoginViewModel(ILibraryModel model)
        {
            _model = model ?? throw new ArgumentNullException("model");

            UserName = String.Empty;

            ExitCommand = new DelegateCommand(param => OnExitApplication());
            LoginCommand = new DelegateCommand(param => LoginAsync(param as PasswordBox));
        }

        private async void LoginAsync(PasswordBox passwordBox)
        {
            if (passwordBox == null)
            {
                return;
            }

            try
            {
                Boolean result = await _model.LoginAsync(UserName, passwordBox.Password);

                if (result)
                {
                    OnLoginSuccess();
                }
                else
                {
                    OnLoginFailed();
                }
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                Exception innerException = ex.InnerException;

                while (innerException != null)
                {
                    msg += Environment.NewLine + innerException.Message;
                    innerException = innerException.InnerException;
                }

                OnMessageApplication($"Unexpected error occurred!{Environment.NewLine}{msg}");
            }
        }

        private void OnLoginSuccess()
        {
            LoginSuccess?.Invoke(this, EventArgs.Empty);
        }

        private void OnExitApplication()
        {
            ExitApplication?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoginFailed()
        {
            LoginFailed?.Invoke(this, EventArgs.Empty);
        }
    }
}
