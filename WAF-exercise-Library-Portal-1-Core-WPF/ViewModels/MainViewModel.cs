using System;
using System.Collections.ObjectModel;
using System.Linq;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;
using WAF_exercise_Library_Portal_1_Core_WPF.Models;

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

        public BookData SelectedBuilding
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

        /// <summary>
        /// Szerkesztett épület lekérdezése.
        /// </summary>
        public BookData EditedBook { get; private set; }

        public DelegateCommand CreateBookCommand { get; private set; }
        public DelegateCommand UpdateBookCommand { get; private set; }
        public DelegateCommand DeleteBookCommand { get; private set; }

        public DelegateCommand UploadImageCommand { get; private set; }
        public DelegateCommand DeleteImageCommand { get; private set; }
        public DelegateCommand AddLinkImageCommand { get; private set; }
        public DelegateCommand RemoveLinkImageCommand { get; private set; }

        public DelegateCommand CreateAuthorCommand { get; private set; }
        public DelegateCommand UpdateAuthorCommand { get; private set; }
        public DelegateCommand DeleteAuthorCommand { get; private set; }
        public DelegateCommand AddLinkAuthorCommand { get; private set; }
        public DelegateCommand RemoveLinkAuthorCommand { get; private set; }

        public DelegateCommand CreateVolumeCommand { get; private set; }
        public DelegateCommand UpdateVolumeCommand { get; private set; }
        public DelegateCommand DeleteVolumeCommand { get; private set; }
        public DelegateCommand AddLinkVolumeCommand { get; private set; }
        public DelegateCommand RemoveVolumeCommand { get; private set; }
        public DelegateCommand ShortOutVolumeCommand { get; private set; }

        public DelegateCommand SaveChangesCommand { get; private set; }
        public DelegateCommand CancelChangesCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }

        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }

        public event EventHandler ExitApplication;

        public MainViewModel(ILibraryModel model)
        {
            _model = model ?? throw new ArgumentNullException("model");

            _model.BookChanged += Model_BuildingChanged;
            _isLoaded = false;

            CreateBookCommand = new DelegateCommand(param =>
            {
                EditedBook = new BookData();  // a szerkesztett épület új lesz
                OnBookEditingStarted();
            });
            UploadImageCommand = new DelegateCommand(param => OnImageEditingStarted((param as BookData).Id));
            UpdateBookCommand = new DelegateCommand(param => UpdateBook(param as BookData));
            DeleteBookCommand = new DelegateCommand(param => DeleteBook(param as BookData));
            DeleteImageCommand = new DelegateCommand(param => DeleteImage(param as CoverData));
            SaveChangesCommand = new DelegateCommand(param => SaveChanges());
            CancelChangesCommand = new DelegateCommand(param => CancelChanges());
            LoadCommand = new DelegateCommand(param => LoadAsync());
            SaveCommand = new DelegateCommand(param => SaveAsync());
            ExitCommand = new DelegateCommand(param => OnExitApplication());
        }

        private void UpdateBook(BookData book)
        {
            if (book == null)
                return;

            EditedBook = new BookData
            {
                Id = book.Id,
                Title = book.Title,
                PublishedYear = book.PublishedYear,
                Isbn = book.Isbn
            };

            OnBookEditingStarted();
        }

        private void DeleteBook(BookData book)
        {
            if (book == null || book.Volumes.Count > 0)
            {
                return;
            }

            _model.DeleteBook(book);
        }

        /// <summary>
        /// Kép törlése.
        /// </summary>
        /// <param name="image">A kép.</param>
        private void DeleteImage(CoverData image)
        {
            if (image == null)
                return;

            _model.DeleteImage(image);
        }

        /// <summary>
        /// Változtatások mentése.
        /// </summary>
        private void SaveChanges()
        {
            // ellenőrzések
            if (String.IsNullOrEmpty(EditedBook.Name))
            {
                OnMessageApplication("Az épületnév nincs megadva!");
                return;
            }
            if (EditedBook.City == null)
            {
                OnMessageApplication("A város nincs megadva!");
                return;
            }
            if (EditedBook.ShoreId == null)
            {
                OnMessageApplication("A tengerpart típus nincs megadva!");
                return;
            }

            // mentés
            if (EditedBook.Id == 0) // ha új az épület
            {
                _model.CreateBuilding(EditedBook);
                Authors.Add(EditedBook);
                SelectedBuilding = EditedBook;
            }
            else // ha már létezik az épület
            {
                _model.UpdateBuilding(EditedBook);
            }

            EditedBook = null;

            OnBuildingEditingFinished();
        }

        /// <summary>
        /// Változtatások elvetése.
        /// </summary>
        private void CancelChanges()
        {
            EditedBook = null;
            OnBuildingEditingFinished();
        }

        /// <summary>
        /// Betöltés végrehajtása.
        /// </summary>
        private async void LoadAsync()
        {
            try
            {
                await _model.LoadAsync();
                Authors = new ObservableCollection<BookData>(_model.Buildings); // az adatokat egy követett gyűjteménybe helyezzük
                Volumes = new ObservableCollection<VolumeData>(_model.Cities);
                IsLoaded = true;
            }
            catch (NetworkException)
            {
                OnMessageApplication("A betöltés sikertelen! Nincs kapcsolat a kiszolgálóval.");
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
                OnMessageApplication("A mentés sikeres!");
            }
            catch (NetworkException)
            {
                OnMessageApplication("A mentés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }

        /// <summary>
        /// Épület megváltozásának eseménykezelése.
        /// </summary>
        private void Model_BuildingChanged(object sender, BuildingEventArgs e)
        {
            Int32 index = Authors.IndexOf(Authors.FirstOrDefault(building => building.Id == e.BuildingId));
            Authors.RemoveAt(index); // módosítjuk a kollekciót
            Authors.Insert(index, _model.Buildings[index]);

            SelectedBuilding = Authors[index]; // és az aktuális épületet
        }

        /// <summary>
        /// Alkalmazásból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitApplication()
        {
            ExitApplication?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Épület szerkesztés elindításának eseménykiváltása.
        /// </summary>
        private void OnBookEditingStarted()
        {
            BookEditingStarted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Épület szerkesztés befejeztének eseménykiváltása.
        /// </summary>
        private void OnBuildingEditingFinished()
        {
            BookEditingFinished?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Képszerkesztés elindításának eseménykiváltása.
        /// </summary>
        /// <param name="buildingId">Épület azonosító.</param>
        private void OnImageEditingStarted(Int32 buildingId)
        {
            ImageUploadingStarted?.Invoke(this, new BuildingEventArgs { BuildingId = buildingId });
        }
    }
}
