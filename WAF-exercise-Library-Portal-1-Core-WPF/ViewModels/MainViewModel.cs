using System;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.Win32;

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
        private Int32 _selectedAddingCoverDataId;
        public Int32 SelectedAddingCoverDataId
        {
            get { return _selectedAddingCoverDataId; }
            set
            {
                if (_selectedAddingCoverDataId != value)
                {
                    _selectedAddingCoverDataId = value;
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

        private ObservableCollection<LendingData> _relevantLendingDatas;
        public ObservableCollection<LendingData> RelevantLendingDatas
        {
            get { return _relevantLendingDatas; }
            private set
            {
                if (_relevantLendingDatas != value)
                {
                    _relevantLendingDatas = value;
                    OnPropertyChanged();
                }
            }
        }
        private LendingData _selectedRelevantLendingData;
        public LendingData SelectedRelevantLendingData
        {
            get { return _selectedRelevantLendingData; }
            set
            {
                if (_selectedRelevantLendingData != value)
                {
                    _selectedRelevantLendingData = value;
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

        public DelegateCommand CreateCoverCommand { get; private set; }
        public DelegateCommand AddCoverCommand { get; private set; }
        public DelegateCommand RemoveCoverCommand { get; private set; }

        public DelegateCommand CreateVolumeCommand { get; private set; }
        public DelegateCommand SortOutVolumeCommand { get; private set; }
        public DelegateCommand DeleteVolumeCommand { get; private set; }

        public DelegateCommand TurnLendingCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }

        public event EventHandler ExitApplication;

        public MainViewModel(ILibraryModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));

            _model.AuthorDataChanged += Model_AuthorDataChanged;
            _model.BookAuthorDataCreated += Model_BookAuthorDataCreated;
            _model.BookAuthorDataDeleted += Model_BookAuthorDataDeleted;
            _model.BookDataChanged += Model_BookDataChanged;
            _model.BookDataDeleted += Model_BookDataDeleted;
            _model.CoverDataChanged += Model_CoverDataChanged;
            _model.CoverDataRemoved += Model_CoverDataRemoved;
            _model.LendingDataChanged += Model_LendingDataChanged;
            _model.LendingDataDeleted += Model_LendingDataDeleted;
            _model.VolumeDataChanged += Model_VolumeDataChanged;
            _model.VolumeDataDeleted += Model_VolumeDataDeleted;

            _isLoaded = false;

            CreateBookCommand = new DelegateCommand(param => CreateBook());
            UpdateBookCommand = new DelegateCommand(param => UpdateBook());
            DeleteBookCommand = new DelegateCommand(param => DeleteBook());

            CreateAuthorCommand = new DelegateCommand(param => CreateAuthor());
            UpdateAuthorCommand = new DelegateCommand(param => UpdateAuthor());
            AddAuthorCommand = new DelegateCommand(param => AddAuthor());
            RemoveAuthorCommand = new DelegateCommand(param => RemoveAuthor());

            CreateCoverCommand = new DelegateCommand((param) => CreateCover());
            AddCoverCommand = new DelegateCommand((param) => AddCover());
            RemoveCoverCommand = new DelegateCommand((param) => RemoveCover());

            CreateVolumeCommand = new DelegateCommand(param => CreateVolume());
            SortOutVolumeCommand = new DelegateCommand(param => SortOutVolume());
            DeleteVolumeCommand = new DelegateCommand(param => DeleteVolume());

            TurnLendingCommand = new DelegateCommand(param => TurnLending());

            LoadCommand = new DelegateCommand(param => LoadAsync());
            SaveCommand = new DelegateCommand(param => SaveAsync());
            ExitCommand = new DelegateCommand(param => OnExitApplication());
        }

        private void CreateBook()
        {
            OnBookEditingStarted(new BookData());
        }
        private void UpdateBook()
        {
            if (SelectedBookData == null || BookDatas.Contains(SelectedBookData) == false)
            {
                OnMessageApplication(String.Format("Failed to UPDATE book.{0}Info: Local sync problem.", Environment.NewLine));
                return;
            }

            OnBookEditingStarted(new BookData(SelectedBookData.Id,
                SelectedBookData.Title,
                SelectedBookData.PublishedYear,
                SelectedBookData.Isbn,
                SelectedBookData.Cover));
        }
        private void DeleteBook()
        {
            if (SelectedBookData == null
             || BookDatas.Contains(SelectedBookData) == false
             || SelectedBookData.AuthorDatas.Any()
             || SelectedBookData.VolumeDatas.Any())
            {
                OnMessageApplication(String.Format("Failed to DELETE book.{0}Info: Local sync problem or still active links to other datas.", Environment.NewLine));
                return;
            }

            try
            {
                _model.DeleteBook(SelectedBookData);
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
        private void Model_BookDataChanged(Object sender, BookData bookData)
        {
            if (bookData == null)
            {
                OnMessageApplication(String.Format("Failed to SYNC book."));
                return;
            }

            Int32 index = BookDatas.IndexOf(BookDatas.FirstOrDefault(b => b.Equals(bookData)));

            if (index != -1)
            {
                BookDatas.RemoveAt(index);
            }
            else
            {
                index = BookDatas.IndexOf(BookDatas.Last());
            }

            BookDatas.Insert(index, bookData);
            SelectedBookData = BookDatas[index];
        }
        private void Model_BookDataDeleted(Object sender, BookData bookData)
        {
            Int32 index = BookDatas.IndexOf(BookDatas.FirstOrDefault(b => b.Equals(bookData)));

            if (index != -1)
            {
                BookDatas.RemoveAt(index);
            }
            else
            {
                OnMessageApplication(String.Format("Failed to SYNC book."));
                return;
            }

            SelectedBookData = null;
        }

        private void CreateAuthor()
        {
            if (SelectedBookData == null
             || BookDatas.Contains(SelectedBookData) == false)
            {
                OnMessageApplication(String.Format("Failed to CREATE author.{0}Info: Local sync problem.", Environment.NewLine));
                return;
            }

            OnAuthorEditingStarted(new AuthorData(), SelectedBookData.Id);
        }
        private void UpdateAuthor()
        {
            if (SelectedBookData == null
             || BookDatas.Contains(SelectedBookData) == false
             || SelectedAuthorData == null
             || BookDatas.First(b => b.Equals(SelectedBookData)).AuthorDatas.Contains(SelectedAuthorData) == false)
            {
                OnMessageApplication(String.Format("Failed to UPDATE author.{0}Info: Local sync problem.", Environment.NewLine));
                return;
            }

            OnAuthorEditingStarted(SelectedAuthorData, SelectedBookData.Id);
        }
        private void AddAuthor()
        {
            if (SelectedBookData == null
             || BookDatas.Contains(SelectedBookData) == false
             || SelectedAddingAuthorData == null
             || AuthorDatas.Contains(SelectedAddingAuthorData) == false
             || SelectedBookData.AuthorDatas.Contains(SelectedAddingAuthorData))
            {
                OnMessageApplication(String.Format("Failed to ADD author.{0}Info: Local sync problem.", Environment.NewLine));
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
        private void RemoveAuthor()
        {
            if (SelectedBookData == null
             || BookDatas.Contains(SelectedBookData) == false
             || SelectedAuthorData == null
             || BookDatas.First(b => b.Equals(SelectedBookData)).AuthorDatas.Contains(SelectedAuthorData) == false)
            {
                OnMessageApplication(String.Format("Failed to REMOVE author.{0}Info: Local sync problem.", Environment.NewLine));
                return;
            }

            try
            {
                _model.RemoveAuthor(SelectedAuthorData.Id, SelectedBookData.Id);
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
        private void Model_AuthorDataChanged(Object sender, AuthorData authorData)
        {
            if (authorData == null)
            {
                OnMessageApplication(String.Format("Failed to SYNC author."));
                return;
            }

            foreach (BookData bookData in authorData.BookDatas)
            {
                Int32 index = BookDatas.IndexOf(BookDatas.FirstOrDefault(b => b.Equals(bookData)));

                if (index != -1)
                {
                    if (SelectedBookData.Equals(bookData))
                    {
                        BookDatas.RemoveAt(index);
                        BookDatas.Insert(index, bookData);

                        SelectedBookData = BookDatas[index];
                    }
                    else
                    {
                        BookDatas.RemoveAt(index);
                        BookDatas.Insert(index, bookData);
                    }
                }
                else
                {
                    OnMessageApplication(String.Format("Failed to SYNC author."));
                }
            }
        }
        private void Model_BookAuthorDataCreated(Object sender, BookAuthorData bookAuthorData)
        {
            UpdateGUIByBook(bookAuthorData.BookData, "book to author");
        }
        private void Model_BookAuthorDataDeleted(Object sender, BookAuthorData bookAuthorData)
        {
            UpdateGUIByBook(bookAuthorData.BookData, "book to author");
        }

        private void CreateCover()
        {
            if (SelectedBookData == null
             || SelectedBookData.Cover != null)
            {
                OnMessageApplication(String.Format("Failed to CREATE cover.{0}Info: Local sync problem.", Environment.NewLine));
                return;
            }

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
                    _model.CreateCover(SelectedBookData.Id, ImageHandler.OpenAndResize(dialog.FileName, 300));

                }
            }
            catch (Exception exception)
            {
                OnMessageApplication(String.Format("Failed to CREATE cover.{0}Info: {1}", Environment.NewLine, exception.Message));
            }
        }
        private void AddCover()
        {
            if (SelectedBookData == null)
            {
                OnMessageApplication(String.Format("Failed to ADD cover.{0}Info: Invalid cover is null.", Environment.NewLine));
                return;
            }

            try
            {
                _model.AddCover(SelectedBookData.Id, SelectedAddingCoverDataId);
            }
            catch (Exception exception)
            {
                OnMessageApplication(String.Format("Failed to ADD cover.{0}Info: {1}", Environment.NewLine, exception.Message));
            }
        }
        private void RemoveCover()
        {
            if (SelectedBookData == null
             || SelectedBookData.Cover == null)
            {
                OnMessageApplication(String.Format("Failed to REMOVE cover.{0}Info: Invalid cover is null.", Environment.NewLine));
                return;
            }

            try
            {
                _model.RemoveCover(SelectedBookData.Id);
            }
            catch (Exception exception)
            {
                OnMessageApplication(String.Format("Failed to REMOVE cover.{0}Info: {1}", Environment.NewLine, exception.Message));
            }

        }
        private void Model_CoverDataChanged(Object sender, BookData bookData)
        {
            UpdateGUIByBook(bookData, "cover");
        }
        private void Model_CoverDataRemoved(Object sender, BookData bookData)
        {
            UpdateGUIByBook(bookData, "cover");
        }

        private void CreateVolume()
        {
            if (SelectedBookData == null)
            {
                OnMessageApplication(String.Format("Failed to CREATE volume.{0}Info: Local sync problem.", Environment.NewLine));
                return;
            }

            try
            {
                _model.CreateVolume(new VolumeData(), SelectedBookData.Id);
            }
            catch (Exception exception)
            {
                OnMessageApplication(String.Format("Failed to CREATE volume.{0}Info: {1}", Environment.NewLine, exception.Message));
            }
        }
        private void SortOutVolume()
        {
            if (SelectedBookData == null
             || SelectedVolumeData == null)
            {
                OnMessageApplication(String.Format("Failed to UPDATE volume.{0}Info: Local sync problem.", Environment.NewLine));
                return;
            }

            VolumeData volumeData = new VolumeData(SelectedVolumeData.Id, !SelectedVolumeData.IsSortedOut);

            try
            {
                _model.UpdateVolume(volumeData);
            }
            catch (Exception exception)
            {
                OnMessageApplication(String.Format("Failed to UPDATE volume.{0}Info: {1}", Environment.NewLine, exception.Message));
            }
        }
        private void DeleteVolume()
        {
            if (SelectedBookData == null
             || SelectedVolumeData == null)
            {
                OnMessageApplication(String.Format("Failed to DELETE volume.{0}Info: Local sync problem.", Environment.NewLine));
                return;
            }

            try
            {
                _model.DeleteVolume(SelectedVolumeData.Id);
            }
            catch (Exception exception)
            {
                OnMessageApplication(String.Format("Failed to UPDATE volume.{0}Info: {1}", Environment.NewLine, exception.Message));
            }
        }
        private void Model_VolumeDataChanged(Object sender, VolumeData volumeData)
        {
            UpdateGUIByBook(volumeData.BookData, "volume");
        }
        private void Model_VolumeDataDeleted(Object sender, VolumeData volumeData)
        {
            UpdateGUIByBook(volumeData.BookData, "volume");
        }

        private void TurnLending()
        {
            if (SelectedRelevantLendingData == null)
            {
                OnMessageApplication(String.Format("Failed to UPDATE lending.{0}Info: Local sync problem.", Environment.NewLine));
                return;
            }

            try
            {
                _model.TurnLending(SelectedRelevantLendingData);
            }
            catch (Exception exception)
            {
                OnMessageApplication(String.Format("Failed to UPDATE volume.{0}Info: {1}", Environment.NewLine, exception.Message));
            }
        }
        private void Model_LendingDataChanged(Object sender, LendingData lendingData)
        {
            if (lendingData == null)
            {
                OnMessageApplication("Failed to SYNC lending.");
                return;
            }

            Int32 index = RelevantLendingDatas.IndexOf(RelevantLendingDatas.FirstOrDefault(ld => ld.Equals(lendingData)));

            if (index != -1)
            {
                RelevantLendingDatas.RemoveAt(index);

                if (lendingData.Active == 2 && lendingData.EndDate < DateTime.UtcNow)
                {
                    SelectedRelevantLendingData = null;
                    return;
                }

                RelevantLendingDatas.Insert(index, lendingData);

                SelectedRelevantLendingData = RelevantLendingDatas[index];
            }
            else
            {
                OnMessageApplication("Failed to SYNC lending.");
            }
        }
        private void Model_LendingDataDeleted(Object sender, LendingData lendingData)
        {
            Int32 index = RelevantLendingDatas.IndexOf(RelevantLendingDatas.FirstOrDefault(ld => ld.Equals(lendingData)));

            if (index != -1)
            {
                RelevantLendingDatas.RemoveAt(index);
            }
            else
            {
                if (lendingData.Active == 1
                || (lendingData.Active == 0 && lendingData.EndDate > DateTime.UtcNow))
                {
                    OnMessageApplication(String.Format("Failed to SYNC lending."));
                }
                return;
            }

            SelectedRelevantLendingData = null;
        }

        private void UpdateGUIByBook(BookData bookData, String what)
        {
            if (bookData == null)
            {
                OnMessageApplication(String.Format("Failed to SYNC {0}.", what));
                return;
            }

            Int32 index = BookDatas.IndexOf(BookDatas.FirstOrDefault(b => b.Equals(bookData)));

            if (index != -1)
            {
                BookDatas.RemoveAt(index);
                BookDatas.Insert(index, bookData);

                SelectedBookData = BookDatas[index];
            }
            else
            {
                OnMessageApplication(String.Format("Failed to SYNC {0}.", what));
            }
        }

        private async void LoadAsync()
        {
            try
            {
                await _model.LoadAsync();

                BookDatas = new ObservableCollection<BookData>(_model.BookDatas);
                AuthorDatas = new ObservableCollection<AuthorData>(_model.AuthorDatas);
                CoverIds = new ObservableCollection<Int32>(_model.CoverDatas.Select(c => c.Id));
                RelevantLendingDatas = new ObservableCollection<LendingData>(_model.LendingDatas.Where(l => l.Active == 1 || (l.EndDate > DateTime.UtcNow && l.Active == 0)));

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
                await _model.LoadAsync();
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
    }
}
