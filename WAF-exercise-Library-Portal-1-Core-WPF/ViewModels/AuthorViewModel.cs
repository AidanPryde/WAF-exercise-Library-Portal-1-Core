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

        public AuthorData EditedAuthor { get; private set; }

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public event EventHandler AuthorEditingFinished;

        public AuthorViewModel(ILibraryModel model, AuthorData authorData, Int32 bookId)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            EditedAuthor = authorData;
            OnPropertyChanged("EditedAuthor");
            _bookId = bookId;

            SaveCommand = new DelegateCommand((param) => Save());
            CancelCommand = new DelegateCommand((param) => Cancel());
        }

        private void Save()
        {
            if (String.IsNullOrEmpty(EditedAuthor.Name))
            {
                OnAuthorEditingFinished();
                return;
            }

            if (EditedAuthor.Id == -1)
            {
                try
                {
                    _model.CreateAuthor(EditedAuthor, _bookId);
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
                    _model.UpdateAuthor(EditedAuthor);
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
