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
        //private BookData _bookData;

        public BookData EditedBook { get; private set; }

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public event EventHandler BookEditingFinished;

        public BookViewModel(ILibraryModel model, BookData bookData)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            EditedBook = bookData;

            SaveCommand = new DelegateCommand((param) =>
            {
                if (EditedBook.Id == -1)
                {
                    _model.CreateBook(EditedBook);
                }
                else
                {
                    _model.UpdateBook(EditedBook);
                }

                OnBookEditingFinished();
            });

            CancelCommand = new DelegateCommand((param) =>
            {
                OnBookEditingFinished();
            });
        }

        private void OnBookEditingFinished()
        {
            BookEditingFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}
