using System;
using System.Collections.ObjectModel;
using System.Linq;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;
using WAF_exercise_Library_Portal_1_Core_WPF.Models;
using WAF_exercise_Library_Portal_1_Core_WPF.Persistence;
using WAF_exercise_Library_Portal_1_Core_WPF.ViewModels.ComplexEventArgs;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ILibraryModel _model;

        private ObservableCollection<BookData> _books;

        private BookData _selectedBook;
        private AuthorData _selectedAuthor;
        private VolumeData _selectedVolume;

        private Boolean _isLoaded;

        public ObservableCollection<BookData> Books
        {
            get { return _books; }
            private set
            {
                if (_books != value)
                {
                    _books = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean IsLoaded
        {
            get { return _isLoaded; }
            private set
            {
                if (_isLoaded != value)
                {
                    _isLoaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public BookData SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                if (_selectedBook != value)
                {
                    _selectedBook = value;
                    OnPropertyChanged();
                }
            }
        }
        public AuthorData SelectedAuthor
        {
            get { return _selectedAuthor; }
            set
            {
                if (_selectedAuthor != value)
                {
                    _selectedAuthor = value;
                    OnPropertyChanged();
                }
            }
        }
        public VolumeData SelectedVolume
        {
            get { return _selectedVolume; }
            set
            {
                if (_selectedVolume != value)
                {
                    _selectedVolume = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand CreateBookCommand { get; private set; }
        public DelegateCommand UpdateBookCommand { get; private set; }
        public DelegateCommand DeleteBookCommand { get; private set; }

        public event EventHandler<BookData> BookEditingStarted;

        public DelegateCommand AddAuthorCommand { get; private set; }
        public DelegateCommand UpdateAuthorCommand { get; private set; }
        public DelegateCommand RemoveAuthorCommand { get; private set; }

        public event EventHandler<AuthorEditingEventArgs> AuthorEditingStarted;

        public DelegateCommand AddCoverCommand { get; private set; }
        public DelegateCommand DeleteCoverCommand { get; private set; }

        public event EventHandler<CoverEditingEventArgs> CoverEditingStarted;

        public DelegateCommand AddVolumeCommand { get; private set; }
        public DelegateCommand UpdateVolumeCommand { get; private set; }
        public DelegateCommand DeleteVolumeCommand { get; private set; }
        public DelegateCommand ShortOutVolumeCommand { get; private set; }

        public event EventHandler<VolumeEditingEventArgs> VolumeEditingStarted;

        public DelegateCommand ExitCommand { get; private set; }

        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }

        public event EventHandler ExitApplication;

        public MainViewModel(ILibraryModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));

            _model.BookChanged += Model_BookChanged;
            _isLoaded = false;

            CreateBookCommand = new DelegateCommand(param => CreateBook());
            UpdateBookCommand = new DelegateCommand(param => UpdateBook(param as BookData));
            DeleteBookCommand = new DelegateCommand(param => DeleteBook(param as BookData));

            AddAuthorCommand = new DelegateCommand(param => AddAuthor());
            UpdateAuthorCommand = new DelegateCommand(param => UpdateAuthor(param as AuthorData));
            RemoveAuthorCommand = new DelegateCommand(param => RemoveAuthor(param as AuthorData));

            AddCoverCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));
            DeleteCoverCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));

            AddVolumeCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));
            UpdateVolumeCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));
            DeleteVolumeCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));
            ShortOutVolumeCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));

            LoadCommand = new DelegateCommand(param => LoadAsync());
            SaveCommand = new DelegateCommand(param => SaveAsync());
            ExitCommand = new DelegateCommand(param => OnExitApplication());
        }

        private void CreateBook()
        {
            OnBookEditingStarted(new BookData());
        }
        private void UpdateBook(BookData bookData)
        {
            if (bookData == null || Books.Contains(bookData) == false)
            {
                return;
            }

            OnBookEditingStarted(new BookData(bookData.Id, bookData.Title, bookData.PublishedYear, bookData.Isbn, bookData.Cover));
        }
        private void DeleteBook(BookData bookData)
        {
            if (bookData == null
             || Books.Contains(bookData) == false
             || bookData.AuthorDatas.Any()
             || bookData.Cover != null
             || bookData.VolumeDatas.Any())
            {
                return;
            }

            try
            {
                _model.DeleteBook(bookData);

                Books.Remove(bookData);
            }
            catch (Exception exception)
            {
                OnMessageApplication(String.Format("Failed to DELETE book.{0}Info: {1}", Environment.NewLine, exception.Message));
            }
        }

        private void OnBookEditingStarted(BookData bookData)
        {
            BookEditingStarted?.Invoke(this, bookData);
        }
        private void Model_BookChanged(Object sender, Int32 e)
        {
            Int32 index = Books.IndexOf(Books.FirstOrDefault(b => b.Id == e));

            if (index != -1)
            {
                Books.RemoveAt(index);
            }
            else
            {
                index = e - 1;
            }

            Books.Insert(index, _model.Books[index]);

            SelectedBook = Books[index];
        }

        private void AddAuthor()
        {
            if (SelectedBook == null || Books.Contains(SelectedBook) == false)
            {
                return;
            }

            OnAuthorEditingStarted(new AuthorData(), SelectedBook.Id);
        }
        private void UpdateAuthor(AuthorData authorData)
        {
            if (SelectedBook == null
             || Books.Contains(SelectedBook) == false
             || authorData == null
             || Books.First(b => b.Equals(SelectedBook.Id)).AuthorDatas.Contains(authorData) == false)
            {
                return;
            }

            OnAuthorEditingStarted(authorData, SelectedBook.Id);
        }
        private void RemoveAuthor(AuthorData authorData)
        {
            if (SelectedBook == null
             || Books.Contains(SelectedBook) == false
             || authorData == null)
            {
                return;
            }

            BookData book = Books.First(b => b.Equals(SelectedBook.Id));

            if (book.AuthorDatas.Contains(authorData) == false)
            {
                return;
            }

            try
            {
                _model.RemoveAuthor(authorData, book.Id);
            }
            catch (Exception exception)
            {
                OnMessageApplication(String.Format("Failed to DELETE book.{0}Info: {1}", Environment.NewLine, exception.Message));
            }
        }

        private void OnAuthorEditingStarted(AuthorData authorData, Int32 bookId)
        {
            AuthorEditingStarted?.Invoke(this, new AuthorEditingEventArgs(authorData, bookId));
        }

        private async void LoadAsync()
        {
            try
            {
                await _model.LoadAsync();

                Books = new ObservableCollection<BookData>(_model.Books);

                IsLoaded = true;
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("Loading failed! No connection to data.");
            }
        }

        private async void SaveAsync()
        {
            try
            {
                await _model.SaveAsync();
                OnMessageApplication("Changes saved successfully.");
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("Saving failed! No connection to data.");
            }
        }

        private void OnExitApplication()
        {
            ExitApplication?.Invoke(this, EventArgs.Empty);
        }

        

        /// <summary>
        /// Képszerkesztés elindításának eseménykiváltása.
        /// </summary>
        /// <param name="buildingId">Épület azonosító.</param>
        private void OnImageEditingStarted(Int32 buildingId)
        {
            //ImageUploadingStarted?.Invoke(this, new BuildingEventArgs { BuildingId = buildingId });
        }
    }
}
