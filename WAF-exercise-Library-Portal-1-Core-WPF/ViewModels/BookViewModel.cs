using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;
using WAF_exercise_Library_Portal_1_Core_WPF.Models;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        private ILibraryModel _model;

        public BookData EditedBook { get; private set; }

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public event EventHandler BookEditingFinished;

        public BookViewModel(ILibraryModel model, BookData bookData)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            EditedBook = bookData;

            SaveCommand = new DelegateCommand((param) => Save());
            CancelCommand = new DelegateCommand((param) => Cancel());
        }

        private void Save()
        {
            if (String.IsNullOrEmpty(EditedBook.Title))
            {
                OnMessageApplication(String.Format("Failed to CREATE book.{0}Info: Invalid TITLE.", Environment.NewLine));
                return;
            }

            if (EditedBook.Isbn.ToString().Length < 8)
            {
                OnMessageApplication(String.Format("Failed to CREATE book.{0}Info: Invalid ISBN.", Environment.NewLine));
                return;
            }

            if (EditedBook.PublishedYear.ToString().Length < 4)
            {
                OnMessageApplication(String.Format("Failed to CREATE book.{0}Info: Invalid PUBLISHEDYEAR.", Environment.NewLine));
                return;
            }

            if (EditedBook.Id == -1)
            {
                try
                {
                    _model.CreateBook(EditedBook);
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
                    _model.UpdateBook(EditedBook);
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
