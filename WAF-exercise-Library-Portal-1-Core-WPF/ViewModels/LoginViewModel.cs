using System;
using System.Windows.Controls;

using WAF_exercise_Library_Portal_1_Core_WPF.Models;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ILibraryModel _model;
        private Boolean _isReady = true;

        public Boolean IsReady
        {
            get { return _isReady; }
            private set
            {
                if (_isReady != value)
                {
                    _isReady = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }

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
            if (IsReady == false)
            {
                return;
            }

            IsReady = false;

            if (passwordBox == null)
            {
                IsReady = true;
                return;
            }

            try
            {
                Boolean result = await _model.LoginAsAdminAsync("admin", "Almafa123");
                //Boolean result = await _model.LoginAsync(UserName, passwordBox.Password);

                if (result)
                {
                    OnLoginSuccess();
                }
                else
                {
                    OnLoginFailed();
                }
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

                OnLoginFailed();
            }

            IsReady = true;
        }

        private void OnLoginSuccess()
        {
            LoginSuccess?.Invoke(this, EventArgs.Empty);
        }
        private void OnLoginFailed()
        {
            LoginFailed?.Invoke(this, EventArgs.Empty);
        }

        private void OnExitApplication()
        {
            ExitApplication?.Invoke(this, EventArgs.Empty);
        }
    }
}
