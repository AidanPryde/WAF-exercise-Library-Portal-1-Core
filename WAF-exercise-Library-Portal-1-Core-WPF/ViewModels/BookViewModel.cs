using System;
using System.Linq;
using System.Collections.ObjectModel;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;
using WAF_exercise_Library_Portal_1_Core_WPF.Models;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        private ILibraryModel _model;

        private BookData _editedBookData;
        public BookData EditedBookData
        {
            get { return _editedBookData; }
            set
            {
                if (_editedBookData != value)
                {
                    _editedBookData = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public event EventHandler BookEditingFinished;

        public BookViewModel(ILibraryModel model, BookData bookData)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _editedBookData = bookData;

            if (_editedBookData.Id == -1)
            {
                _editedBookData.Isbn = 123456789;
                _editedBookData.Title = "TestTitle";
                _editedBookData.PublishedYear = 2000;
            }

            SaveCommand = new DelegateCommand((param) => Save());
            CancelCommand = new DelegateCommand((param) => Cancel());
        }

        private void Save()
        {
            if (String.IsNullOrEmpty(EditedBookData.Title))
            {
                OnMessageApplication(String.Format("Failed to CREATE book.{0}Info: Invalid TITLE.", Environment.NewLine));
                return;
            }

            if (EditedBookData.Isbn.ToString().Length < 8)
            {
                OnMessageApplication(String.Format("Failed to CREATE book.{0}Info: Invalid ISBN.", Environment.NewLine));
                return;
            }

            if (EditedBookData.PublishedYear.ToString().Length < 4)
            {
                OnMessageApplication(String.Format("Failed to CREATE book.{0}Info: Invalid PUBLISHEDYEAR.", Environment.NewLine));
                return;
            }

            if (EditedBookData.Id == -1)
            {
                try
                {
                    _model.CreateBook(EditedBookData);
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
                    _model.UpdateBook(EditedBookData);
                }
                catch (Exception exception)
                {
                    OnMessageApplication(String.Format("Failed to UPDATE book.{0}Info: {1}", Environment.NewLine, exception.Message));
                    return;
                }
            }

            OnBookEditingFinished();
        }

        private void Cancel()
        {
            OnBookEditingFinished();
        }

        private void OnBookEditingFinished()
        {
            BookEditingFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}
