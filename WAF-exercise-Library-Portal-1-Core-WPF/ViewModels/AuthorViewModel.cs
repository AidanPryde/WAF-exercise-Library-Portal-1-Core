using System;
using System.Collections.ObjectModel;
using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;
using WAF_exercise_Library_Portal_1_Core_WPF.Models;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels
{
    public class AuthorViewModel : BaseViewModel
    {
        private ILibraryModel _model;

        private Int32 _bookId;

        private AuthorData _editedAuthorData;
        public AuthorData EditedAuthorData
        {
            get { return _editedAuthorData; }
            set
            {
                if (_editedAuthorData != value)
                {
                    _editedAuthorData = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public event EventHandler AuthorEditingFinished;

        public AuthorViewModel(ILibraryModel model, AuthorData authorData, Int32 bookId)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _bookId = bookId;
            _editedAuthorData = authorData;

            if (_editedAuthorData.Id == -1)
            {
                _editedAuthorData.Name = "TestAuthor";
            }

            SaveCommand = new DelegateCommand((param) => Save());
            CancelCommand = new DelegateCommand((param) => Cancel());
        }

        private void Save()
        {
            if (String.IsNullOrEmpty(EditedAuthorData.Name))
            {
                OnAuthorEditingFinished();
                return;
            }

            if (EditedAuthorData.Id == -1)
            {
                try
                {
                    _model.CreateAuthor(EditedAuthorData, _bookId);
                }
                catch (Exception exception)
                {
                    OnMessageApplication(String.Format("Failed to CREATE book.{0}Info: {1}", Environment.NewLine, exception.Message));
                    return;
                }
            }
            else
            {
                try
                {
                    _model.UpdateAuthor(EditedAuthorData);
                }
                catch (Exception exception)
                {
                    OnMessageApplication(String.Format("Failed to UPDATE book.{0}Info: {1}", Environment.NewLine, exception.Message));
                    return;
                }
            }

            OnAuthorEditingFinished();
        }

        private void Cancel()
        {
            OnAuthorEditingFinished();
        }

        private void OnAuthorEditingFinished()
        {
            AuthorEditingFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}
