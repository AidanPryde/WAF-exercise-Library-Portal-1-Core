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

        private ObservableCollection<BookData> _bookDatas;
        public ObservableCollection<BookData> BookDatas
        {
            get { return _bookDatas; }
            private set
            {
                if (_bookDatas != value)
                {
                    _bookDatas = value;
                    OnPropertyChanged();
                }
            }
        }
        private BookData _selectedBookData;
        public BookData SelectedBookData
        {
            get { return _selectedBookData; }
            set
            {
                if (_selectedBookData != value)
                {
                    _selectedBookData = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<AuthorData> _authorDatas;
        public ObservableCollection<AuthorData> AuthorDatas
        {
            get { return _authorDatas; }
            private set
            {
                if (_authorDatas != value)
                {
                    _authorDatas = value;
                    OnPropertyChanged();
                }
            }
        }
        private AuthorData _selectedAuthorData;
        public AuthorData SelectedAuthorData
        {
            get { return _selectedAuthorData; }
            set
            {
                if (_selectedAuthorData != value)
                {
                    _selectedAuthorData = value;
                    OnPropertyChanged();
                }
            }
        }
        private AuthorData _selectedAddingAuthorData;
        public AuthorData SelectedAddingAuthorData
        {
            get { return _selectedAddingAuthorData; }
            set
            {
                if (_selectedAddingAuthorData != value)
                {
                    _selectedAddingAuthorData = value;
                    OnPropertyChanged();
                }
            }
        }

        private VolumeData _selectedVolumeData;
        public VolumeData SelectedVolumeData
        {
            get { return _selectedVolumeData; }
            set
            {
                if (_selectedVolumeData != value)
                {
                    _selectedVolumeData = value;
                    OnPropertyChanged();
                }
            }
        }

        private Boolean _isLoaded;
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

        public DelegateCommand CreateBookCommand { get; private set; }
        public DelegateCommand UpdateBookCommand { get; private set; }
        public DelegateCommand DeleteBookCommand { get; private set; }

        public event EventHandler<BookData> BookEditingStarted;

        public DelegateCommand CreateAuthorCommand { get; private set; }
        public DelegateCommand UpdateAuthorCommand { get; private set; }
        public DelegateCommand AddAuthorCommand { get; private set; }
        public DelegateCommand RemoveAuthorCommand { get; private set; }

        public event EventHandler<AuthorEditingEventArgs> AuthorEditingStarted;

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

            _model.BookDatasChanged += Model_BookChanged;
            _isLoaded = false;

            CreateBookCommand = new DelegateCommand(param => CreateBook());
            UpdateBookCommand = new DelegateCommand(param => UpdateBook(param as BookData));
            DeleteBookCommand = new DelegateCommand(param => DeleteBook(param as BookData));


            CreateAuthorCommand = new DelegateCommand(param => CreateAuthor());
            UpdateAuthorCommand = new DelegateCommand(param => UpdateAuthor(param as AuthorData));
            AddAuthorCommand = new DelegateCommand(param => AddAuthor());
            RemoveAuthorCommand = new DelegateCommand(param => RemoveAuthor(param as AuthorData));

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
            if (bookData == null || BookDatas.Contains(bookData) == false)
            {
                OnMessageApplication(String.Format("Failed to DELETE book.{0}Info: Local sync problem.", Environment.NewLine));
                return;
            }

            OnBookEditingStarted(new BookData(bookData.Id, bookData.Title, bookData.PublishedYear, bookData.Isbn, bookData.Cover));
        }
        private void DeleteBook(BookData bookData)
        {
            if (bookData == null
             || BookDatas.Contains(bookData) == false
             || bookData.AuthorDatas.Any()
             || bookData.VolumeDatas.Any())
            {
                OnMessageApplication(String.Format("Failed to DELETE book.{0}Info: Local sync problem or still active links to other datas.", Environment.NewLine));
                return;
            }

            try
            {
                _model.DeleteBook(bookData);

                BookDatas.Remove(bookData);
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
            Int32 index = BookDatas.IndexOf(BookDatas.FirstOrDefault(b => b.Id == e));

            if (index != -1)
            {
                BookDatas.RemoveAt(index);
            }
            else
            {
                index = e - 1;
            }

            BookDatas.Insert(index, _model.BookDatas[index]);

            SelectedBookData = BookDatas[index];
        }

        private void CreateAuthor()
        {
            if (SelectedBookData == null || BookDatas.Contains(SelectedBookData) == false)
            {
                return;
            }

            OnAuthorEditingStarted(new AuthorData(), SelectedBookData.Id);
        }
        private void UpdateAuthor(AuthorData authorData)
        {
            if (SelectedBookData == null
             || BookDatas.Contains(SelectedBookData) == false
             || authorData == null
             || BookDatas.First(b => b.Equals(SelectedBookData)).AuthorDatas.Contains(authorData) == false)
            {
                return;
            }

            OnAuthorEditingStarted(authorData, SelectedBookData.Id);
        }
        private void AddAuthor()
        {
            if (SelectedBookData == null || BookDatas.Contains(SelectedBookData) == false
             || SelectedAddingAuthorData == null || AuthorDatas.Contains(SelectedAddingAuthorData) == false
             || SelectedBookData.AuthorDatas.Contains(SelectedAddingAuthorData))
            {
                return;
            }

            try
            {
                _model.AddAuthor(SelectedAddingAuthorData.Id, SelectedBookData.Id);
            }
            catch (Exception exception)
            {
                OnMessageApplication(String.Format("Failed to ADD author.{0}Info: {1}", Environment.NewLine, exception.Message));
            }
            
        }
        private void RemoveAuthor(AuthorData authorData)
        {
            if (SelectedBookData == null
             || BookDatas.Contains(SelectedBookData) == false
             || authorData == null)
            {
                return;
            }

            BookData book = BookDatas.First(b => b.Equals(SelectedBookData));

            if (book.AuthorDatas.Contains(authorData) == false)
            {
                return;
            }

            try
            {
                _model.RemoveAuthor(authorData.Id, book.Id);
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

                BookDatas = new ObservableCollection<BookData>(_model.BookDatas);
                AuthorDatas = new ObservableCollection<AuthorData>(_model.AuthorDatas);

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
