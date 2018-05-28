using System;
using System.Linq;
using System.Collections.ObjectModel;

using Microsoft.Win32;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;
using WAF_exercise_Library_Portal_1_Core_WPF.Models;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        private ILibraryModel _model;

        public BookData EditedBook { get; private set; }

        private ObservableCollection<Int32> _coverIds;
        public ObservableCollection<Int32> CoverIds
        {
            get { return _coverIds; }
            private set
            {
                if (_coverIds != value)
                {
                    _coverIds = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 SelectedCoverId { get; set; }

        public DelegateCommand CreateCoverCommand { get; private set; }
        public DelegateCommand AddCoverCommand { get; private set; }
        public DelegateCommand RemoveCoverCommand { get; private set; }

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public event EventHandler BookEditingFinished;

        public BookViewModel(ILibraryModel model, BookData bookData)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            EditedBook = bookData;

            _coverIds = new ObservableCollection<Int32>(_model.CoverDatas.Select(c => c.Id));

            CreateCoverCommand = new DelegateCommand((param) => Create());
            AddCoverCommand = new DelegateCommand((param) => Add());
            RemoveCoverCommand = new DelegateCommand((param) => Remove());

            SaveCommand = new DelegateCommand((param) => Save());
            CancelCommand = new DelegateCommand((param) => Cancel());
        }

        private void Create()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    CheckFileExists = true,
                    Filter = "Images|*.jpg;*.jpeg;*.bmp;*.tif;*.gif;*.png;",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
                };
                Boolean? result = dialog.ShowDialog();

                if (result == true)
                {
                    CoverData newCoverData = _model.CreateCover(EditedBook.Id,
                                            ImageHandler.OpenAndResize(dialog.FileName, 430));

                    CoverIds.Add(newCoverData.Id);
                    EditedBook.Cover = newCoverData;
                    OnPropertyChanged("EditedBook");
                }
            }
            catch(Exception exception)
            {
                OnMessageApplication(String.Format("Failed to CREATE cover.{0}Info: {1}", Environment.NewLine, exception.Message));
            }
        }

        private void Add()
        {
            try
            {
                CoverData coverData = _model.AddCover(EditedBook.Id, SelectedCoverId);
                EditedBook.Cover = coverData;
                OnPropertyChanged("EditedBook");
            }
            catch (Exception exception)
            {
                OnMessageApplication(String.Format("Failed to ADD cover.{0}Info: {1}", Environment.NewLine, exception.Message));
            }
        }

        private void Remove()
        {
            if (EditedBook.Cover == null)
            {
                OnMessageApplication(String.Format("Failed to REMOVE cover.{0}Info: Invalid cover is null.", Environment.NewLine));
                return;
            }

            try
            {
                _model.RemoveCover(EditedBook.Id);
                EditedBook.Cover = null;
                OnPropertyChanged("EditedBook");
            }
            catch (Exception exception)
            {
                OnMessageApplication(String.Format("Failed to REMOVE cover.{0}Info: {1}", Environment.NewLine, exception.Message));
            }
            
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
