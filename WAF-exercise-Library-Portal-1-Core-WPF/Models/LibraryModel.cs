using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using WAF_exercise_Library_Portal_1_Core_Db.Models;
using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;
using WAF_exercise_Library_Portal_1_Core_WPF.Persistence;

namespace WAF_exercise_Library_Portal_1_Core_WPF.Models
{
    public class LibraryModel : ILibraryModel
    {
        private enum DataStateFlag
        {
            Created,
            Added,
            Updated,
            Removed,
            Deleted
        }

        private ILibraryPersistence _persistence;

        private List<BookData> _bookDatas;
        public IReadOnlyList<BookData> BookDatas { get { return _bookDatas; } }
        private Dictionary<BookData, DataStateFlag> _bookDatasFlags;
        public event EventHandler<Int32> BookDatasChanged;
        public event EventHandler<Int32> BookDatasDeleted;

        private List<BookAuthorData> _bookAuthorDatas;
        public IReadOnlyList<BookAuthorData> BookAuthorDatas { get { return _bookAuthorDatas; } }
        private Dictionary<BookAuthorData, DataStateFlag> _bookAuthorsDatasFlags;

        private List<AuthorData> _authorDatas;
        public IReadOnlyList<AuthorData> AuthorDatas { get { return _authorDatas; } }
        private Dictionary<AuthorData, DataStateFlag> _authorsDatasFlags;
        public event EventHandler<Int32> AuthorDataAdded;

        private List<CoverData> _coverDatas;
        public IReadOnlyList<CoverData> CoverDatas { get { return _coverDatas; } }
        private Dictionary<CoverData, DataStateFlag> _coversDatasFlags;
        public event EventHandler<Int32> CoverDataAdded;

        private List<VolumeData> _volumeDatas;
        public IReadOnlyList<VolumeData> VolumeDatas { get { return _volumeDatas; } }
        private Dictionary<VolumeData, DataStateFlag> _volumesDatasFlags;


        private List<LendingData> _lendingDatas;
        public IReadOnlyList<LendingData> LendingDatas { get { return _lendingDatas; } }
        private Dictionary<LendingData, DataStateFlag> _lendingsDatasFlags;
        public event EventHandler<Int32> LendingDataChanged;

        public Boolean IsUserLoggedIn { get; private set; }

        public LibraryModel(ILibraryPersistence persistence)
        {
            IsUserLoggedIn = false;
            _persistence = persistence;
        }

        public async Task<Boolean> LoginAsync(string name, string password)
        {
            // The API will handle the already logged in user case.

            LoginData user = new LoginData
            {
                UserName = name,
                Password = password
            };

            try
            {
                IsUserLoggedIn = await _persistence.LoginAsync(user);
            }
            catch (Exception)
            {
            }

            return IsUserLoggedIn;
        }
        public async Task<Boolean> LogoutAsync()
        {
            if (IsUserLoggedIn == false)
            {
                return true;
            }

            try
            {
                IsUserLoggedIn = !(await _persistence.LogoutAsync());
            }
            catch (Exception)
            {
            }

            return IsUserLoggedIn;
        }

        private void OnBookDatasChanged(Int32 bookId)
        {
            BookDatasChanged?.Invoke(this, bookId);
        }
        private void OnBookDatasDeleted(Int32 bookId)
        {
            BookDatasDeleted?.Invoke(this, bookId);
        }
        public void CreateBook(BookData bookData)
        {
            if (bookData == null)
            {
                throw new ArgumentNullException(nameof(bookData));
            }

            if (_bookDatas.Contains(bookData))
            {
                throw new ArgumentException("The book is already in the collection.", nameof(bookData));
            }

            bookData.Id = (_bookDatas.Count > 0 ? _bookDatas.Max(b => b.Id) : 0) + 1;
            
            _bookDatas.Add(bookData);
            _bookDatasFlags.Add(bookData, DataStateFlag.Created);

            OnBookDatasChanged(bookData.Id);
        }
        public void UpdateBook(BookData bookData)
        {
            if (bookData == null)
            {
                throw new ArgumentNullException(nameof(bookData));
            }

            BookData oldBookData = _bookDatas.FirstOrDefault(b => b.Equals(bookData))
                ?? throw new ArgumentException("The book does not exist.", nameof(bookData));

            oldBookData.Title = bookData.Title;
            oldBookData.PublishedYear = bookData.PublishedYear;
            oldBookData.Isbn = bookData.Isbn;

            if (_bookDatasFlags.ContainsKey(oldBookData) == false
             || _bookDatasFlags[oldBookData] == DataStateFlag.Deleted)
            {
                _bookDatasFlags[oldBookData] = DataStateFlag.Updated;
            }

            OnBookDatasChanged(bookData.Id);
        }
        public void DeleteBook(BookData bookData)
        {
            if (bookData == null)
            {
                throw new ArgumentNullException(nameof(bookData));
            }

            BookData bookToDelete = _bookDatas.FirstOrDefault(b => b.Equals(bookData))
                ?? throw new ArgumentException("The book does not exist.", nameof(bookData));

            if (bookToDelete.AuthorDatas == null
             || bookToDelete.VolumeDatas == null)
            {
                throw new Exception("Invalid book data.");
            }

            if (bookToDelete.Cover != null
             || bookToDelete.AuthorDatas.Any()
             || bookToDelete.VolumeDatas.Any())
            {
                throw new ArgumentException("The book still has links to other datas.", nameof(bookData));
            }

            _bookDatas.Remove(bookToDelete);
            OnBookDatasDeleted(bookToDelete.Id);

            if (_bookDatasFlags.ContainsKey(bookToDelete)
             && _bookDatasFlags[bookToDelete] == DataStateFlag.Created)
            {
                _bookDatasFlags.Remove(bookToDelete);
            }
            else
            {
                _bookDatasFlags[bookToDelete] = DataStateFlag.Deleted;
            }
        }

        public void CreateBookAuthor(BookAuthorData bookAuthorData)
        {
            if (bookAuthorData == null)
            {
                throw new ArgumentNullException(nameof(bookAuthorData));
            }

            if (_bookAuthorDatas.Contains(bookAuthorData))
            {
                throw new ArgumentException("BookAuthor already in the collection.", nameof(bookAuthorData));
            }

            if (bookAuthorData.AuthorData == null
             || _authorDatas.Contains(bookAuthorData.AuthorData) == false
             || bookAuthorData.BookData == null
             || _bookDatas.Contains(bookAuthorData.BookData) == false)
            {
                throw new ArgumentException("BookAuthor try to link to invalid data.", nameof(bookAuthorData));
            }

            _bookAuthorDatas.Add(bookAuthorData);
            _bookAuthorsDatasFlags[bookAuthorData] = DataStateFlag.Created;

            if (bookAuthorData.AuthorData.BookDatas == null
             || bookAuthorData.BookData.AuthorDatas == null)
            {
                throw new ArgumentException("Invalid BookAuthor data.", nameof(bookAuthorData));
            }

            bookAuthorData.AuthorData.BookDatas.Add(bookAuthorData.BookData);
            OnAuthorDatasAdded(bookAuthorData.AuthorData.Id);

            bookAuthorData.BookData.AuthorDatas.Add(bookAuthorData.AuthorData);
            OnBookDatasChanged(bookAuthorData.BookData.Id);
        }
        public void DeleteBookAuthor(Int32 bookAuthorId)
        {
            BookAuthorData bookAuthorData = _bookAuthorDatas.FirstOrDefault(bad => bad.Id == bookAuthorId)
                ?? throw new ArgumentException("BookAuthor not in the collection.", nameof(bookAuthorId));

            if (bookAuthorData.AuthorData == null
             || _authorDatas.Contains(bookAuthorData.AuthorData) == false
             || bookAuthorData.BookData == null
             || _bookDatas.Contains(bookAuthorData.BookData) == false)
            {
                throw new ArgumentException("BookAuthor holds invalid data.", nameof(bookAuthorId));
            }

            if (bookAuthorData.AuthorData.BookDatas == null
             || bookAuthorData.BookData.AuthorDatas == null)
            {
                throw new ArgumentException("Invalid BookAuthor data.", nameof(bookAuthorData));
            }

            bookAuthorData.AuthorData.BookDatas.Remove(bookAuthorData.BookData);
            OnAuthorDatasAdded(bookAuthorData.AuthorData.Id);

            bookAuthorData.BookData.AuthorDatas.Remove(bookAuthorData.AuthorData);
            OnBookDatasChanged(bookAuthorData.BookData.Id);

            _bookAuthorDatas.Remove(bookAuthorData);
            if (_bookAuthorsDatasFlags.ContainsKey(bookAuthorData)
             && _bookAuthorsDatasFlags[bookAuthorData] == DataStateFlag.Created)
            {
                _bookAuthorsDatasFlags.Remove(bookAuthorData);
            }
            else
            {
                _bookAuthorsDatasFlags[bookAuthorData] = DataStateFlag.Deleted;
            }
        }

        private void OnAuthorDatasAdded(Int32 authorId)
        {
            AuthorDataAdded?.Invoke(this, authorId);
        }
        public void CreateAuthor(AuthorData authorData, Int32 bookId)
        {
            if (authorData == null)
            {
                throw new ArgumentNullException(nameof(authorData));
            }

            BookData bookData = _bookDatas.FirstOrDefault(b => b.Id == bookId)
                ?? throw new ArgumentException("No book in the collection.", nameof(bookId));

            if (_authorDatas.Contains(authorData))
            {
                throw new ArgumentException("Author already in the author collection.", nameof(authorData));
            }

            authorData.Id = (_authorDatas.Count > 0 ? _authorDatas.Max(b => b.Id) : 0) + 1;
            _authorDatas.Add(authorData);
            _authorsDatasFlags.Add(authorData, DataStateFlag.Created);

            try
            {
                CreateBookAuthor(new BookAuthorData((_bookAuthorDatas.Count > 0 ? _bookAuthorDatas.Max(bad => bad.Id) : 0) + 1,
                    bookData,
                    authorData));
            }
            catch (Exception)
            {
                _authorDatas.Remove(authorData);
                _authorsDatasFlags.Remove(authorData);
                throw;
            }
        }
        public void AddAuthor(Int32 authorId, Int32 bookId)
        {
            AuthorData authorData = _authorDatas.FirstOrDefault(b => b.Id == authorId)
                ?? throw new ArgumentException("The author does not exist.", nameof(authorId));

            BookData bookData = _bookDatas.FirstOrDefault(b => b.Id == bookId)
                ?? throw new ArgumentException("No book in the collection.", nameof(bookId));

            if (authorData.BookDatas == null)
            {
                throw new Exception("Invalid book data.");
            }

            if (authorData.BookDatas.Contains(bookData))
            {
                throw new Exception("Book already in the book collection of the author.");
            }

            if (bookData.AuthorDatas == null)
            {
                throw new Exception("Invalid author data.");
            }

            if (bookData.AuthorDatas.Contains(authorData))
            {
                throw new Exception("Author already in the author collection of the book.");
            }

            if (_bookAuthorDatas.FirstOrDefault(bad => bad.AuthorData?.Id == authorId && bad.BookData?.Id == bookId) != null)
            {
                throw new Exception("BookAuthor already in the collection.");
            }

            CreateBookAuthor(new BookAuthorData((_bookAuthorDatas.Count > 0 ? _bookAuthorDatas.Max(b => b.Id) : 0) + 1,
                bookData,
                authorData));
        }
        public void UpdateAuthor(AuthorData authorData)
        {
            if (authorData == null)
            {
                throw new ArgumentNullException(nameof(authorData));
            }

            AuthorData oldAuthorData = _authorDatas.FirstOrDefault(b => b.Equals(authorData))
                ?? throw new ArgumentException("The author does not exist.", nameof(authorData));

            oldAuthorData.Name = authorData.Name;

            if (_authorsDatasFlags.ContainsKey(oldAuthorData) == false
             || _authorsDatasFlags[oldAuthorData] == DataStateFlag.Deleted)
            {
                _authorsDatasFlags[oldAuthorData] = DataStateFlag.Updated;
            }
            OnAuthorDatasAdded(oldAuthorData.Id);

            if (oldAuthorData.BookDatas == null)
            {
                throw new Exception("Ivalid author data.");
            }
        }
        public void RemoveAuthor(Int32 authorId, Int32 bookId)
        {
            AuthorData authorData = _authorDatas.FirstOrDefault(b => b.Id == authorId)
                ?? throw new ArgumentException("The author does not exist.", nameof(authorId));

            BookData bookData = _bookDatas.FirstOrDefault(b => b.Id == bookId)
                ?? throw new ArgumentException("No book in the collection.", nameof(bookId));

            if (authorData.BookDatas == null)
            {
                throw new Exception("Invalid book data.");
            }

            if (authorData.BookDatas.Contains(bookData) == false)
            {
                throw new Exception("Book not in the book collection of the author.");
            }

            if (bookData.AuthorDatas == null)
            {
                throw new Exception("Invalid author data.");
            }

            if (bookData.AuthorDatas.Contains(authorData) == false)
            {
                throw new Exception("Author not in the author collection of the book.");
            }

            BookAuthorData bookAuthorData = _bookAuthorDatas.FirstOrDefault(bad => bad.AuthorData?.Id == authorId && bad.BookData?.Id == bookId)
                ?? throw new Exception("BookAuthor not in the collection.");

            DeleteBookAuthor(bookAuthorData.Id);
        }

        private void OnCoverDatasAdded(Int32 coverId)
        {
            CoverDataAdded?.Invoke(this, coverId);
        }
        public CoverData CreateCover(Int32 bookId, Byte[] image)
        {
            BookData bookData = _bookDatas.FirstOrDefault(b => b.Id == bookId)
                ?? throw new Exception(String.Format("No book found with id: {0}", bookId));

            Int32 coverId = (_coverDatas.Count > 0 ? _bookDatas.Max(b => b.Id) : 0) + 1;
            _coverDatas.Add(new CoverData(coverId, image, bookData));
            OnCoverDatasAdded(coverId);

            bookData.Cover = _coverDatas.FirstOrDefault(c => c.Id == coverId)
                ?? throw new Exception(String.Format("No cover found with id: {0}", coverId));
            OnBookDatasChanged(bookData.Id);

            if (_bookDatasFlags.ContainsKey(bookData) == false
             || _bookDatasFlags[bookData] == DataStateFlag.Deleted)
            {
                _bookDatasFlags[bookData] = DataStateFlag.Updated;
            }
            _coversDatasFlags[bookData.Cover] = DataStateFlag.Created;

            return bookData.Cover;
        }
        public CoverData AddCover(Int32 bookId, Int32 coverId)
        {
            BookData bookData = _bookDatas.FirstOrDefault(b => b.Id == bookId)
                ?? throw new Exception(String.Format("No book found with id: {0}", bookId));

            CoverData coverData = _coverDatas.FirstOrDefault(c => c.Id == coverId)
                ?? throw new Exception(String.Format("No cover found with id: {0}", coverId));

            if (coverData.BookDatas == null)
            {
                throw new Exception(String.Format("Invalid coverData: {0}", coverId));
            }

            bookData.Cover = coverData;
            OnBookDatasChanged(bookData.Id);

            coverData.BookDatas.Add(bookData);
            OnCoverDatasAdded(coverData.Id);

            if (_bookDatasFlags.ContainsKey(bookData) == false
             || _bookDatasFlags[bookData] == DataStateFlag.Deleted)
            {
                _bookDatasFlags[bookData] = DataStateFlag.Updated;
            }

            return coverData;
        }
        public void RemoveCover(Int32 bookId)
        {
            BookData bookData = _bookDatas.FirstOrDefault(b => bookId == b.Id)
                ?? throw new Exception(String.Format("No book found with id: {0}", bookId));

            if (bookData.Cover == null
             || bookData.Cover.BookDatas == null
             || bookData.Cover.BookDatas.Contains(bookData) == false)
            {
                throw new Exception(String.Format("Invalid book with id: {0}", bookId));
            }

            Int32 coverId = bookData.Cover.Id;

            bookData.Cover.BookDatas.Remove(bookData);
            OnCoverDatasAdded(coverId);

            bookData.Cover = null;
            OnBookDatasChanged(bookData.Id);

            if (_bookDatasFlags.ContainsKey(bookData) == false
             || _bookDatasFlags[bookData] == DataStateFlag.Deleted)
            {
                _bookDatasFlags[bookData] = DataStateFlag.Updated;
            }
        }

        public void CreateVolume(VolumeData volumeData, Int32 bookId)
        {
            if (volumeData == null)
            {
                throw new ArgumentNullException(nameof(volumeData));
            }

            if (_volumeDatas.Contains(volumeData))
            {
                throw new ArgumentException("The volume is already in the collection.", nameof(volumeData));
            }

            BookData bookData = _bookDatas.FirstOrDefault(b => b.Id == bookId)
                ?? throw new Exception(String.Format("No book found with id: {0}", bookId));

            volumeData.Id = Guid.NewGuid().ToString();
            volumeData.BookData = bookData;
            OnBookDatasChanged(bookData.Id);

            _volumeDatas.Add(volumeData);

            _volumesDatasFlags.Add(volumeData, DataStateFlag.Created);
        }
        public void UpdateVolume(VolumeData volumeData)
        {
            VolumeData oldVolumeData = _volumeDatas.FirstOrDefault(v => v.Equals(volumeData))
                ?? throw new ArgumentException(String.Format("No book found.", volumeData));

            if (oldVolumeData.BookData == null)
            {
                throw new Exception(String.Format("Invalid volume without book: {0}", oldVolumeData));
            }

            BookData bookData = _bookDatas.FirstOrDefault(b => b.Id == oldVolumeData.BookData?.Id)
                ?? throw new Exception(String.Format("No book found with id: {0}", oldVolumeData.BookData.Id));

            if (volumeData.IsSortedOut == true)
            {
                foreach (LendingData lendingData in volumeData.LendingDatas)
                {
                    if (IsStopingSortingOutVolumeLending(lendingData))
                    {
                        throw new Exception(String.Format("Volume can not be sorted out: {0}", oldVolumeData));
                    }

                    if (GetLendingDataState(lendingData) != LendingState.RETURNED)
                    {
                        DeleteLending(lendingData.Id);
                    }
                }
            }
            oldVolumeData.IsSortedOut = volumeData.IsSortedOut;

            if (_volumesDatasFlags.ContainsKey(oldVolumeData) == false
             || _volumesDatasFlags[oldVolumeData] == DataStateFlag.Deleted)
            {
                _volumesDatasFlags[oldVolumeData] = DataStateFlag.Updated;
            }
        }
        public void DeleteVolume(String volumeDataId)
        {
            VolumeData volumeData = _volumeDatas.FirstOrDefault(v => v.Id == volumeDataId)
                ?? throw new ArgumentException("Volume is not in the collection.", nameof(volumeDataId));

            if (volumeData.BookData == null
            || _bookDatas.Contains(volumeData.BookData) == false
            || volumeData.BookData.VolumeDatas == null)
            {
                throw new ArgumentException("Volume holds invalid data.", nameof(volumeData));
            }

            if (volumeData.LendingDatas != null
             && volumeData.LendingDatas.Count > 0)
            {
                throw new ArgumentException("Volume lending collection holds data.", nameof(volumeData));
            }

            volumeData.BookData.VolumeDatas.Remove(volumeData);
            OnBookDatasChanged(volumeData.BookData.Id);

            _volumeDatas.Remove(volumeData);

            if (_volumesDatasFlags.ContainsKey(volumeData)
             && _volumesDatasFlags[volumeData] == DataStateFlag.Created)
            {
                _volumesDatasFlags.Remove(volumeData);
            }
            else
            {
                _volumesDatasFlags[volumeData] = DataStateFlag.Deleted;
            }
        }

        private void OnLendingDatasChanged(Int32 lendingId)
        {
            LendingDataChanged?.Invoke(this, lendingId);
        }
        public void TurnLending(LendingData lendingData)
        {
            LendingData oldLendingData = _lendingDatas.FirstOrDefault(v => v.Equals(lendingData))
                ?? throw new ArgumentException(String.Format("No lending found.", lendingData));

            if (oldLendingData.VolumeData == null)
            {
                throw new Exception(String.Format("Invalid lending without book: {0}", oldLendingData));
            }

            VolumeData volumeData = _volumeDatas.FirstOrDefault(b => b.Id == oldLendingData.VolumeData?.Id)
                ?? throw new Exception(String.Format("No volume found with id: {0}", oldLendingData.VolumeData.Id));

            if (IsLendableLending(lendingData))
            {
                oldLendingData.Active = true;
            }
            else if (IsReturnableLending(lendingData))
            {
                oldLendingData.Active = false;
            }
            else
            {
                throw new Exception(String.Format("Lending can not be turned: {0}", oldLendingData.Id));
            }
            
            OnLendingDatasChanged(oldLendingData.Id);

            if (_lendingsDatasFlags.ContainsKey(oldLendingData) == false
             || _lendingsDatasFlags[oldLendingData] == DataStateFlag.Deleted)
            {
                _lendingsDatasFlags[oldLendingData] = DataStateFlag.Updated;
            }
        }
        public void DeleteLending(Int32 lendingDataId)
        {
            LendingData lendingData = _lendingDatas.FirstOrDefault(v => v.Id == lendingDataId)
                ?? throw new ArgumentException("Lending is not in the collection.", nameof(lendingDataId));

            if (IsStopingSortingOutVolumeLending(lendingData))
            {
                throw new Exception(String.Format("Lending can not be DELETED: {0}", lendingData));
            }

            if (lendingData.VolumeData == null
            || _volumeDatas.Contains(lendingData.VolumeData) == false
            || lendingData.VolumeData.LendingDatas == null
            || lendingData.VolumeData.LendingDatas.Contains(lendingData) == false)
            {
                throw new ArgumentException("Lending holds invalid data.", nameof(lendingData));
            }

            lendingData.VolumeData.LendingDatas.Remove(lendingData);
            OnLendingDatasChanged(lendingData.Id);

            _lendingDatas.Remove(lendingData);

            if (_lendingsDatasFlags.ContainsKey(lendingData)
             && _lendingsDatasFlags[lendingData] == DataStateFlag.Created)
            {
                _lendingsDatasFlags.Remove(lendingData);
            }
            else
            {
                _lendingsDatasFlags[lendingData] = DataStateFlag.Deleted;
            }
        }

        public async Task LoadAsync()
        {
            _bookDatas = (await _persistence.ReadBooksAsync()).ToList();
            _bookDatasFlags = new Dictionary<BookData, DataStateFlag>();

            _bookAuthorDatas = (await _persistence.ReadBookAuthorsAsync()).ToList();
            _bookAuthorsDatasFlags = new Dictionary<BookAuthorData, DataStateFlag>();

            _authorDatas = (await _persistence.ReadAuthorsAsync()).ToList();
            _authorsDatasFlags = new Dictionary<AuthorData, DataStateFlag>();

            _coverDatas = (await _persistence.ReadCoversAsync()).ToList();
            _coversDatasFlags = new Dictionary<CoverData, DataStateFlag>();

            _volumeDatas = (await _persistence.ReadVolumesAsync()).ToList();
            _volumesDatasFlags = new Dictionary<VolumeData, DataStateFlag>();

            _lendingDatas = (await _persistence.ReadLendingsAsync()).ToList();
            _lendingsDatasFlags = new Dictionary<LendingData, DataStateFlag>();

            LinkingDataLists();
        }
        private void LinkingDataLists()
        {
            foreach (BookAuthorData bookAuthorData in _bookAuthorDatas)
            {
                AuthorData authorData = _authorDatas.FirstOrDefault(ad => ad.Equals(bookAuthorData.AuthorData))
                    ?? throw new Exception("The author does not exist.");
                bookAuthorData.AuthorData = authorData;

                BookData bookData = _bookDatas.FirstOrDefault(bd => bd.Equals(bookAuthorData.BookData))
                    ?? throw new Exception("The book does not exist.");
                bookAuthorData.BookData = bookData;
            }

            foreach (BookData bookData in _bookDatas)
            {
                if (bookData.Cover != null)
                {
                    CoverData coverData = _coverDatas.FirstOrDefault(cd => cd.Equals(bookData.Cover))
                        ?? throw new Exception("The cover does not exist.");
                    bookData.Cover = coverData;

                    coverData.BookDatas.Add(bookData);
                }

                bookData.AuthorDatas = new List<AuthorData>(_bookAuthorDatas.Where(bad => bad.BookData.Equals(bookData)).Select(bad => bad.AuthorData));
                bookData.VolumeDatas = new List<VolumeData>(_volumeDatas.Where(vd => vd.BookData.Equals(bookData)));
            }

            foreach (AuthorData authorData in _authorDatas)
            {
                authorData.BookDatas = new List<BookData>(_bookAuthorDatas.Where(bad => bad.AuthorData.Equals(authorData)).Select(bad => bad.BookData));
            }

            foreach (VolumeData volumeData in _volumeDatas)
            {
                volumeData.LendingDatas = new List<LendingData>(_lendingDatas.Where(ld => ld.VolumeData.Equals(volumeData)));
            }

            foreach (LendingData lendingData in _lendingDatas)
            {
                lendingData.VolumeData = _volumeDatas.FirstOrDefault(vd => vd.Equals(lendingData.VolumeData))
                    ?? throw new Exception("The volume does not exist.");
            }
        }

        public async Task SaveAsync()
        {
            await SaveBooksAsync();
            await SaveAuthorsAsync();
        }
        private async Task SaveBooksAsync()
        {
            List<BookData> booksToSave = _bookDatasFlags.Keys.ToList();

            foreach (BookData bookData in booksToSave)
            {
                Boolean result = true;

                switch (_bookDatasFlags[bookData])
                {
                    case DataStateFlag.Created:
                        result = await _persistence.CreateBookAsync(bookData);
                        break;
                    case DataStateFlag.Updated:
                        result = await _persistence.UpdateBookAsync(bookData);
                        break;
                    case DataStateFlag.Deleted:
                        result = await _persistence.DeleteBookAsync(bookData);
                        break;
                    default:
                        throw new InvalidOperationException("Operation " + _bookDatasFlags[bookData] + " is invalid on book " + bookData.Id);
                }

                if (!result)
                {
                    throw new InvalidOperationException("Operation " + _bookDatasFlags[bookData] + " failed on book " + bookData.Id);
                }

                _bookDatasFlags.Remove(bookData);
            }
        }
        private async Task SaveAuthorsAsync()
        {
            List<AuthorData> authorsToSave = _authorsDatasFlags.Keys.ToList();

            foreach (AuthorData authorData in authorsToSave)
            {
                Boolean result = true;

                switch (_authorsDatasFlags[authorData])
                {
                    case DataStateFlag.Created:
                        result = await _persistence.AddAuthorAsync(authorData);
                        break;
                    case DataStateFlag.Updated:
                        result = await _persistence.UpdateAuthorAsync(authorData);
                        break;
                    case DataStateFlag.Deleted:
                        result = await _persistence.RemoveAuthorAsync(authorData);
                        break;
                }

                if (!result)
                {
                    throw new InvalidOperationException("Operation " + _authorsDatasFlags[authorData] + " failed on author " + authorData.Id);
                }

                _authorsDatasFlags.Remove(authorData);
            }
        }

        private LendingState GetLendingDataState(LendingData lendingData)
        {
            DateTime now = DateTime.UtcNow;

            if (now < lendingData.StartDate)
            {
                if (lendingData.Active == false)
                {
                    return LendingState.TOO_SOON_TO_PICK_UP;
                }
                else
                {
                    return LendingState.ERROR;
                }
            }

            if (now < lendingData.EndDate)
            {
                if (lendingData.Active == false)
                {
                    return LendingState.READY_TO_PICK_UP;
                }
                else if (lendingData.Active == true)
                {
                    return LendingState.PICKED_UP;
                }
                else
                {
                    return LendingState.RETURNED;
                }
            }

            if (lendingData.Active == false)
            {
                return LendingState.NOT_PICKED_UP;
            }
            else if (lendingData.Active == true)
            {
                return LendingState.NOT_RETURNED;
            }
            else
            {
                return LendingState.RETURNED;
            }
        }
        private Boolean IsStopingSortingOutVolumeLending(LendingData lendingData)
        {
            LendingState lendingState = GetLendingDataState(lendingData);

            if (lendingState == LendingState.ERROR
             || lendingState == LendingState.NOT_RETURNED
             || lendingState == LendingState.PICKED_UP)
            {
                return true;
            }

            return false;
        }
        private Boolean IsReturnableLending(LendingData lendingData)
        {
            LendingState lendingState = GetLendingDataState(lendingData);

            if (lendingState == LendingState.NOT_RETURNED
             || lendingState == LendingState.PICKED_UP)
            {
                return true;
            }

            return false;
        }
        private Boolean IsLendableLending(LendingData lendingData)
        {
            LendingState lendingState = GetLendingDataState(lendingData);

            if (lendingState == LendingState.READY_TO_PICK_UP)
            {
                return true;
            }

            if (lendingState == LendingState.TOO_SOON_TO_PICK_UP)
            {
                foreach (LendingData otherLendingData in lendingData.VolumeData.LendingDatas)
                {
                    LendingState otherLendingState = GetLendingDataState(otherLendingData);

                    if (otherLendingState == LendingState.NOT_RETURNED
                     || otherLendingState == LendingState.PICKED_UP
                     || otherLendingState == LendingState.READY_TO_PICK_UP)
                    {
                        return false;
                    }

                    if (otherLendingState == LendingState.TOO_SOON_TO_PICK_UP
                     && otherLendingData != lendingData)
                    {
                        if (otherLendingData.StartDate < lendingData.StartDate)
                        {
                            return false;
                        }
                    }
                }
            }

            return false;
        }
    }
}
