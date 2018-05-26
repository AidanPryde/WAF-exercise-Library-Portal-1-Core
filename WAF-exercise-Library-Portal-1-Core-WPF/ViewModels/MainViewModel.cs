using System;
using System.Collections.ObjectModel;
using System.Linq;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;
using WAF_exercise_Library_Portal_1_Core_WPF.Models;
using WAF_exercise_Library_Portal_1_Core_WPF.Persistence;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ILibraryModel _model;

        private ObservableCollection<BookData> _books;
        private ObservableCollection<AuthorData> _authors;
        private ObservableCollection<VolumeData> _volumes;

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
        public ObservableCollection<AuthorData> Authors
        {
            get { return _authors; }
            private set
            {
                if (_authors != value)
                {
                    _authors = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<VolumeData> Volumes
        {
            get { return _volumes; }
            private set
            {
                if (_volumes != value)
                {
                    _volumes = value;
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

        public DelegateCommand AddImageCommand { get; private set; }
        public DelegateCommand DeleteImageCommand { get; private set; }

        public DelegateCommand AddVolumeCommand { get; private set; }
        public DelegateCommand UpdateVolumeCommand { get; private set; }
        public DelegateCommand DeleteVolumeCommand { get; private set; }
        public DelegateCommand ShortOutVolumeCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }

        public event EventHandler ExitApplication;

        public MainViewModel(ILibraryModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));

            _model.BookChanged += Model_BookChanged;
            _isLoaded = false;

            CreateBookCommand = new DelegateCommand(param =>
            {
                OnBookEditingStarted(new BookData());
            });

            UpdateBookCommand = new DelegateCommand(param =>
            {
                BookData book = param as BookData;

                if (book == null || !Books.Contains(book))
                {
                    return;
                }

                OnBookEditingStarted(new BookData(book.Id, book.Title, book.PublishedYear, book.Isbn));
            });

            DeleteBookCommand = new DelegateCommand(param =>
            {
                BookData book = param as BookData;

                if (book == null || !Books.Contains(book))
                {
                    return;
                }

                _model.DeleteBook(book.Id);
            });

            AddAuthorCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));
            UpdateAuthorCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));
            RemoveAuthorCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));

            AddImageCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));
            DeleteImageCommand = new DelegateCommand(param => DeleteImage(param as CoverData));

            AddVolumeCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));
            UpdateVolumeCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));
            DeleteVolumeCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));
            ShortOutVolumeCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));

            LoadCommand = new DelegateCommand(param => LoadAsync());
            SaveCommand = new DelegateCommand(param => SaveAsync());
            ExitCommand = new DelegateCommand(param => OnExitApplication());
        }

        /// <summary>
        /// Kép törlése.
        /// </summary>
        /// <param name="image">A kép.</param>
        private void DeleteImage(CoverData image)
        {
            
        }

        /// <summary>
        /// Betöltés végrehajtása.
        /// </summary>
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

        /// <summary>
        /// Mentés végreahajtása.
        /// </summary>
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

        private void Model_BookChanged(object sender, Int32 e)
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

        private void OnExitApplication()
        {
            ExitApplication?.Invoke(this, EventArgs.Empty);
        }

        private void OnBookEditingStarted(BookData bookData)
        {
            BookEditingStarted?.Invoke(this, bookData);
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
