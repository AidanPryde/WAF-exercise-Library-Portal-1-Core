using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;

using WAF_exercise_Library_Portal_1_Core_WPF.Persistence;
using WAF_exercise_Library_Portal_1_Core_WPF.Models;
using WAF_exercise_Library_Portal_1_Core_WPF.Views;
using WAF_exercise_Library_Portal_1_Core_WPF.ViewModels;
using WAF_exercise_Library_Portal_1_Core_WPF.ViewModels.ComplexEventArgs;

namespace WAF_exercise_Library_Portal_1_Core_WPF
{
    public partial class App : Application
    {
        private ILibraryModel _model;

        private LoginViewModel _loginViewModel;
        private LoginWindow _loginView;

        private MainViewModel _mainViewModel;
        private MainWindow _mainView;

        private BookViewModel _bookViewModel;
        private BookWindow _bookView;

        private AuthorViewModel _authorViewModel;
        private AuthorWindow _authorView;

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new LibraryModel(new LibraryPersistence(ConfigurationManager.AppSettings["baseAddress"]));

            _loginViewModel = new LoginViewModel(_model);

            _loginViewModel.ExitApplication += ViewModel_ExitApplication;
            _loginViewModel.MessageApplication += ViewModel_MessageApplication;
            _loginViewModel.LoginSuccess += LoginViewModel_LoginSuccess;
            _loginViewModel.LoginFailed += LoginViewModel_LoginFailed;

            _loginView = new LoginWindow()
            {
                DataContext = _loginViewModel
            };
            _loginView.Show();
        }

        public async void App_Exit(object sender, ExitEventArgs e)
        {
            if (_model.IsUserLoggedIn)
            {
                await _model.LogoutAsync();
            }
        }

        private void ViewModel_ExitApplication(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Library", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void LoginViewModel_LoginSuccess(object sender, EventArgs e)
        {
            _mainViewModel = new MainViewModel(_model);
            _mainViewModel.ExitApplication += ViewModel_ExitApplication;
            _mainViewModel.MessageApplication += ViewModel_MessageApplication;
            _mainViewModel.BookEditingStarted += MainViewModel_BookEditingStarted;
            _mainViewModel.AuthorEditingStarted += MainViewModel_AuthorEditingStarted;

            _mainView = new MainWindow
            {
                DataContext = _mainViewModel
            };

            _mainView.Show();

            _loginView.Close();
        }

        private void LoginViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("Login failed!", "Library", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MainViewModel_BookEditingStarted(object sender, BookData bookData)
        {
            _bookViewModel = new BookViewModel(_model, bookData);
            _bookViewModel.BookEditingFinished += BookViewModel_BookEditingFinished;
            _bookViewModel.MessageApplication += ViewModel_MessageApplication;

            _bookView = new BookWindow
            {
                DataContext = _bookViewModel
            };

            _bookView.ShowDialog();
        }
        private void BookViewModel_BookEditingFinished(object sender, EventArgs e)
        {
            _bookViewModel.BookEditingFinished -= BookViewModel_BookEditingFinished;
            _bookViewModel.MessageApplication -= ViewModel_MessageApplication;

            _bookView.Close();
        }

        private void MainViewModel_AuthorEditingStarted(object sender, AuthorEditingEventArgs e)
        {
            _authorViewModel = new AuthorViewModel(_model, e.AuthorData, e.BookId);
            _authorViewModel.AuthorEditingFinished += AuthorViewModel_BookEditingFinished;
            _authorViewModel.MessageApplication += ViewModel_MessageApplication;

            _authorView = new AuthorWindow
            {
                DataContext = _authorViewModel
            };

            _authorView.ShowDialog();
        }
        private void AuthorViewModel_BookEditingFinished(object sender, EventArgs e)
        {
            _authorViewModel.AuthorEditingFinished -= AuthorViewModel_BookEditingFinished;
            _authorViewModel.MessageApplication -= ViewModel_MessageApplication;

            _authorView.Close();
        }
    }
}
