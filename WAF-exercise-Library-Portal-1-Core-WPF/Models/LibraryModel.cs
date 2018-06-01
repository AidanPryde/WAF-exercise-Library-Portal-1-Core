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
            Updated,
            Deleted
        }

        private ILibraryPersistence _persistence;

        private List<BookData> _bookDatas;
        public IReadOnlyList<BookData> BookDatas { get { return _bookDatas; } }
        private Dictionary<BookData, DataStateFlag> _bookDatasFlags;
        public event EventHandler<BookData> BookDataChanged;
        public event EventHandler<BookData> BookDataDeleted;

        private List<BookAuthorData> _bookAuthorDatas;
        public IReadOnlyList<BookAuthorData> BookAuthorDatas { get { return _bookAuthorDatas; } }
        private Dictionary<BookAuthorData, DataStateFlag> _bookAuthorDatasFlags;
        public event EventHandler<BookAuthorData> BookAuthorDataCreated;
        public event EventHandler<BookAuthorData> BookAuthorDataDeleted;

        private List<AuthorData> _authorDatas;
        public IReadOnlyList<AuthorData> AuthorDatas { get { return _authorDatas; } }
        private Dictionary<AuthorData, DataStateFlag> _authorDatasFlags;
        public event EventHandler<AuthorData> AuthorDataChanged;

        private List<CoverData> _coverDatas;
        public IReadOnlyList<CoverData> CoverDatas { get { return _coverDatas; } }
        private Dictionary<CoverData, DataStateFlag> _coverDatasFlags;
        public event EventHandler<BookData> CoverDataChanged;
        public event EventHandler<BookData> CoverDataRemoved;

        private List<VolumeData> _volumeDatas;
        public IReadOnlyList<VolumeData> VolumeDatas { get { return _volumeDatas; } }
        private Dictionary<VolumeData, DataStateFlag> _volumeDatasFlags;
        public event EventHandler<VolumeData> VolumeDataChanged;
        public event EventHandler<VolumeData> VolumeDataDeleted;

        private List<LendingData> _lendingDatas;
        public IReadOnlyList<LendingData> LendingDatas { get { return _lendingDatas; } }
        private Dictionary<LendingData, DataStateFlag> _lendingDatasFlags;
        public event EventHandler<LendingData> LendingDataChanged;
        public event EventHandler<LendingData> LendingDataDeleted;

        public Boolean IsUserLoggedIn { get; private set; }

        public LibraryModel(ILibraryPersistence persistence)
        {
            IsUserLoggedIn = false;
            _persistence = persistence;
        }

        public async Task<Boolean> LoginAsAdminAsync(string name, string password)
        {
            // The API will handle the already logged in user case.

            LoginData user = new LoginData
            {
                UserName = name,
                Password = password
            };

            try
            {
                IsUserLoggedIn = await _persistence.LoginAsAdminAsync(user);
            }
            catch (Exception exception)
            {
                IsUserLoggedIn = false;
                throw exception;
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

        private void OnBookDataChanged(BookData bookData)
        {
            BookDataChanged?.Invoke(this, bookData);
        }
        private void OnBookDataDeleted(BookData bookData)
        {
            BookDataDeleted?.Invoke(this, bookData);
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

            OnBookDataChanged(bookData);
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

            if (_bookDatasFlags.ContainsKey(oldBookData)
                && _bookDatasFlags[oldBookData] == DataStateFlag.Created)
            {
                _bookDatasFlags[oldBookData] = DataStateFlag.Created;
            }
            else
            {
                _bookDatasFlags[oldBookData] = DataStateFlag.Updated;
            }

            OnBookDataChanged(bookData);
        }
        public void DeleteBook(BookData bookData)
        {
            if (bookData == null)
            {
                throw new ArgumentNullException(nameof(bookData));
            }

            BookData oldBookData = _bookDatas.FirstOrDefault(b => b.Equals(bookData))
                ?? throw new ArgumentException("The book does not exist.", nameof(bookData));

            if (oldBookData.AuthorDatas == null
             || oldBookData.VolumeDatas == null)
            {
                throw new Exception("Invalid book data.");
            }

            if (oldBookData.Cover != null
             || oldBookData.AuthorDatas.Any()
             || oldBookData.VolumeDatas.Any())
            {
                throw new ArgumentException("The book still has links to other datas.", nameof(bookData));
            }

            _bookDatas.Remove(oldBookData);
            

            if (_bookDatasFlags.ContainsKey(oldBookData)
             && _bookDatasFlags[oldBookData] == DataStateFlag.Created)
            {
                _bookDatasFlags.Remove(oldBookData);
            }
            else
            {
                _bookDatasFlags[oldBookData] = DataStateFlag.Deleted;
            }

            OnBookDataDeleted(oldBookData);
        }

        private void OnBookAuthorDataCreated(BookAuthorData bookAuthorData)
        {
            BookAuthorDataCreated?.Invoke(this, bookAuthorData);
        }
        private void OnBookAuthorDataDeleted(BookAuthorData bookAuthorData)
        {
            BookAuthorDataDeleted?.Invoke(this, bookAuthorData);
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

            if (bookAuthorData.AuthorData.BookDatas == null
             || bookAuthorData.BookData.AuthorDatas == null)
            {
                throw new ArgumentException("Invalid BookAuthor data.", nameof(bookAuthorData));
            }

            bookAuthorData.AuthorData.BookDatas.Add(bookAuthorData.BookData);
            bookAuthorData.BookData.AuthorDatas.Add(bookAuthorData.AuthorData);

            _bookAuthorDatas.Add(bookAuthorData);
            _bookAuthorDatasFlags[bookAuthorData] = DataStateFlag.Created;

            OnBookAuthorDataCreated(bookAuthorData);
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
            bookAuthorData.BookData.AuthorDatas.Remove(bookAuthorData.AuthorData);

            _bookAuthorDatas.Remove(bookAuthorData);

            if (_bookAuthorDatasFlags.ContainsKey(bookAuthorData)
             && _bookAuthorDatasFlags[bookAuthorData] == DataStateFlag.Created)
            {
                _bookAuthorDatasFlags.Remove(bookAuthorData);
            }
            else
            {
                _bookAuthorDatasFlags[bookAuthorData] = DataStateFlag.Deleted;
            }

            OnBookAuthorDataDeleted(bookAuthorData);
        }

        private void OnAuthorDataChanged(AuthorData authorData)
        {
            AuthorDataChanged?.Invoke(this, authorData);
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
            _authorDatasFlags.Add(authorData, DataStateFlag.Created);

            try
            {
                CreateBookAuthor(new BookAuthorData((_bookAuthorDatas.Count > 0 ? _bookAuthorDatas.Max(bad => bad.Id) : 0) + 1,
                    bookData,
                    authorData));
            }
            catch(Exception exception)
            {
                _authorDatas.Remove(authorData);
                _authorDatasFlags.Remove(authorData);

                throw exception;
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

            if (oldAuthorData.BookDatas == null)
            {
                throw new Exception("Ivalid author data.");
            }

            oldAuthorData.Name = authorData.Name;

            if (_authorDatasFlags.ContainsKey(oldAuthorData) == false
             || _authorDatasFlags[oldAuthorData] == DataStateFlag.Deleted)
            {
                _authorDatasFlags[oldAuthorData] = DataStateFlag.Updated;
            }

            OnAuthorDataChanged(oldAuthorData);
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

        private void OnCoverDataChanged(BookData bookData)
        {
            CoverDataChanged?.Invoke(this, bookData);
        }
        private void OnCoverDataRemoved(BookData bookData)
        {
            CoverDataRemoved?.Invoke(this, bookData);
        }
        public void CreateCover(Int32 bookId, Byte[] image)
        {
            BookData bookData = _bookDatas.FirstOrDefault(b => b.Id == bookId)
                ?? throw new Exception(String.Format("No book found with id: {0}", bookId));

            Int32 coverId = (_coverDatas.Count > 0 ? _bookDatas.Max(b => b.Id) : 0) + 1;
            _coverDatas.Add(new CoverData(coverId, image, bookData));

            bookData.Cover = _coverDatas.FirstOrDefault(c => c.Id == coverId)
                ?? throw new Exception(String.Format("No cover found with id: {0}", coverId));

            if (_bookDatasFlags.ContainsKey(bookData) == false
             || _bookDatasFlags[bookData] == DataStateFlag.Deleted)
            {
                _bookDatasFlags[bookData] = DataStateFlag.Updated;
            }
            _coverDatasFlags[bookData.Cover] = DataStateFlag.Created;

            OnCoverDataChanged(bookData);
        }
        public void AddCover(Int32 bookId, Int32 coverId)
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
            coverData.BookDatas.Add(bookData);

            if (_bookDatasFlags.ContainsKey(bookData) == false
             || _bookDatasFlags[bookData] == DataStateFlag.Deleted)
            {
                _bookDatasFlags[bookData] = DataStateFlag.Updated;
            }

            OnCoverDataChanged(bookData);
        }
        public void RemoveCover(Int32 bookId)
        {
            BookData bookData = _bookDatas.FirstOrDefault(bd => bookId == bd.Id)
                ?? throw new Exception(String.Format("No book found with id: {0}", bookId));

            if (bookData.Cover == null
             || bookData.Cover.BookDatas == null
             || bookData.Cover.BookDatas.Contains(bookData) == false)
            {
                throw new Exception(String.Format("Invalid book with id: {0}", bookId));
            }
            bookData.Cover.BookDatas.Remove(bookData);
            bookData.Cover = null;

            if (_bookDatasFlags.ContainsKey(bookData) == false
             || _bookDatasFlags[bookData] == DataStateFlag.Deleted)
            {
                _bookDatasFlags[bookData] = DataStateFlag.Updated;
            }

            OnCoverDataRemoved(bookData);
        }

        private void OnVolumeDataChanged(VolumeData volumeData)
        {
            VolumeDataChanged?.Invoke(this, volumeData);
        }
        private void OnVolumeDataDeleted(VolumeData volumeData)
        {
            VolumeDataDeleted?.Invoke(this, volumeData);
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

            BookData bookData = _bookDatas.FirstOrDefault(bd => bd.Id == bookId)
                ?? throw new Exception(String.Format("No book found with id: {0}", bookId));
            if (bookData.VolumeDatas == null)
            {
                throw new Exception(String.Format("Invalid book: {0}", bookData));
            }

            volumeData.Id = Guid.NewGuid().ToString();
            volumeData.BookData = bookData;

            if (_volumeDatas.Contains(volumeData) == true)
            {
                throw new Exception(String.Format("Volume already exists: {0}", volumeData));
            }

            _volumeDatas.Add(volumeData);

            if (bookData.VolumeDatas.Contains(volumeData) == true)
            {
                throw new Exception(String.Format("Volume already exists: {0}", bookData));
            }

            bookData.VolumeDatas.Add(volumeData);

            _volumeDatasFlags.Add(volumeData, DataStateFlag.Created);

            OnVolumeDataChanged(volumeData);
        }
        public void UpdateVolume(VolumeData volumeData)
        {
            VolumeData oldVolumeData = _volumeDatas.FirstOrDefault(v => v.Equals(volumeData))
                ?? throw new ArgumentException(String.Format("No book found.", volumeData));

            if (oldVolumeData.BookData == null)
            {
                throw new Exception(String.Format("Invalid volume without book: {0}", oldVolumeData));
            }

            BookData bookData = _bookDatas.FirstOrDefault(bd => bd.Id == oldVolumeData.BookData?.Id)
                ?? throw new Exception(String.Format("No book found with id: {0}", oldVolumeData.BookData.Id));

            if (volumeData.IsSortedOut == true)
            {
                List<Int32> lendingDataIdsToDelete = new List<Int32>();
                foreach (LendingData lendingData in oldVolumeData.LendingDatas)
                {
                    if (IsStopingSortingOutVolumeLending(lendingData))
                    {
                        throw new Exception(String.Format("Volume can not be sorted out: {0}", oldVolumeData));
                    }

                    if (GetLendingDataState(lendingData) != LendingState.RETURNED)
                    {
                        lendingDataIdsToDelete.Add(lendingData.Id);
                    }
                }

                foreach (Int32 lendingDataId in lendingDataIdsToDelete)
                {
                    DeleteLending(lendingDataId);
                }
            }

            oldVolumeData.IsSortedOut = volumeData.IsSortedOut;

            if (_volumeDatasFlags.ContainsKey(oldVolumeData) == false
             || _volumeDatasFlags[oldVolumeData] == DataStateFlag.Deleted)
            {
                _volumeDatasFlags[oldVolumeData] = DataStateFlag.Updated;
            }

            OnVolumeDataChanged(oldVolumeData);
        }
        public void DeleteVolume(String volumeDataId)
        {
            VolumeData volumeData = _volumeDatas.FirstOrDefault(vd => vd.Id == volumeDataId)
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
            _volumeDatas.Remove(volumeData);

            if (_volumeDatasFlags.ContainsKey(volumeData)
             && _volumeDatasFlags[volumeData] == DataStateFlag.Created)
            {
                _volumeDatasFlags.Remove(volumeData);
            }
            else
            {
                _volumeDatasFlags[volumeData] = DataStateFlag.Deleted;
            }

            OnVolumeDataChanged(volumeData);
        }

        private void OnLendingDataChanged(LendingData lendingData)
        {
            LendingDataChanged?.Invoke(this, lendingData);
        }
        private void OnLendingDataDeleted(LendingData lendingData)
        {
            LendingDataDeleted?.Invoke(this, lendingData);
        }
        public void TurnLending(LendingData lendingData)
        {
            LendingData oldLendingData = _lendingDatas.FirstOrDefault(v => v.Equals(lendingData))
                ?? throw new ArgumentException(String.Format("No lending found.", lendingData));

            if (oldLendingData.VolumeData == null)
            {
                throw new Exception(String.Format("Invalid lending without book: {0}", oldLendingData));
            }

            VolumeData volumeData = _volumeDatas.FirstOrDefault(vd => vd.Id == oldLendingData.VolumeData?.Id)
                ?? throw new Exception(String.Format("No volume found with id: {0}", oldLendingData.VolumeData.Id));

            if (IsLendableLending(lendingData))
            {
                oldLendingData.Active = 1;
                oldLendingData.StartDate = DateTime.UtcNow;
            }
            else if (IsReturnableLending(lendingData))
            {
                oldLendingData.Active = 2;
                oldLendingData.EndDate = DateTime.UtcNow.AddSeconds(-1);
            }
            else
            {
                throw new Exception(String.Format("Lending can not be turned: {0}", oldLendingData.Id));
            }

            if (_lendingDatasFlags.ContainsKey(oldLendingData) == false
             || _lendingDatasFlags[oldLendingData] == DataStateFlag.Deleted)
            {
                _lendingDatasFlags[oldLendingData] = DataStateFlag.Updated;
            }

            OnLendingDataChanged(oldLendingData);
        }
        public void DeleteLending(Int32 lendingDataId)
        {
            LendingData lendingData = _lendingDatas.FirstOrDefault(ld => ld.Id == lendingDataId)
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

            _lendingDatas.Remove(lendingData);

            if (_lendingDatasFlags.ContainsKey(lendingData)
             && _lendingDatasFlags[lendingData] == DataStateFlag.Created)
            {
                _lendingDatasFlags.Remove(lendingData);
            }
            else
            {
                _lendingDatasFlags[lendingData] = DataStateFlag.Deleted;
            }

            OnLendingDataDeleted(lendingData);
        }

        private LendingState GetLendingDataState(LendingData lendingData)
        {
            DateTime now = DateTime.UtcNow;

            if (now < lendingData.StartDate)
            {
                if (lendingData.Active == 0)
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
                if (lendingData.Active == 0)
                {
                    return LendingState.READY_TO_PICK_UP;
                }
                else if (lendingData.Active == 1)
                {
                    return LendingState.PICKED_UP;
                }
                else
                {
                    return LendingState.RETURNED;
                }
            }

            if (lendingData.Active == 0)
            {
                return LendingState.NOT_PICKED_UP;
            }
            else if (lendingData.Active == 1)
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
                return true;
            }

            return false;
        }

        public async Task LoadAsync()
        {
            _bookDatas = (await _persistence.ReadBooksAsync()).ToList();
            _bookDatasFlags = new Dictionary<BookData, DataStateFlag>();

            _bookAuthorDatas = (await _persistence.ReadBookAuthorsAsync()).ToList();
            _bookAuthorDatasFlags = new Dictionary<BookAuthorData, DataStateFlag>();

            _authorDatas = (await _persistence.ReadAuthorsAsync()).ToList();
            _authorDatasFlags = new Dictionary<AuthorData, DataStateFlag>();

            _coverDatas = (await _persistence.ReadCoversAsync()).ToList();
            _coverDatasFlags = new Dictionary<CoverData, DataStateFlag>();

            _volumeDatas = (await _persistence.ReadVolumesAsync()).ToList();
            _volumeDatasFlags = new Dictionary<VolumeData, DataStateFlag>();

            _lendingDatas = (await _persistence.ReadLendingsAsync()).ToList();
            _lendingDatasFlags = new Dictionary<LendingData, DataStateFlag>();

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

                BookData bookData = _bookDatas.FirstOrDefault(bd => bd.Equals(volumeData.BookData))
                        ?? throw new Exception("The book does not exist.");

                volumeData.BookData = bookData;
            }

            foreach (LendingData lendingData in _lendingDatas)
            {
                lendingData.VolumeData = _volumeDatas.FirstOrDefault(vd => vd.Equals(lendingData.VolumeData))
                    ?? throw new Exception("The volume does not exist.");
            }
        }

        public async Task SaveAsync()
        {
            await SaveAuthorsAsync();
            await SaveCoverAsync();
            await SaveBooksAsync();
            await SaveBookAuthorAsync();
            await SaveVolumeAsync();
            await SaveLendingAsync();
        }

        private async Task SaveAuthorsAsync()
        {
            List<AuthorData> authorsToSave = _authorDatasFlags.Keys.ToList();

            foreach (AuthorData authorData in authorsToSave)
            {
                Boolean result = true;

                switch (_authorDatasFlags[authorData])
                {
                    case DataStateFlag.Created:
                        result = await _persistence.CreateAuthorAsync(authorData);
                        break;
                    case DataStateFlag.Updated:
                        result = await _persistence.UpdateAuthorAsync(authorData);
                        break;
                    case DataStateFlag.Deleted:
                        break;
                }

                if (!result)
                {
                    throw new InvalidOperationException("Operation " + _authorDatasFlags[authorData] + " failed on author " + authorData.Id);
                }

                _authorDatasFlags.Remove(authorData);
            }
        }
        private async Task SaveCoverAsync()
        {
            List<CoverData> coversToSave = _coverDatasFlags.Keys.ToList();

            foreach (CoverData coverData in coversToSave)
            {
                Boolean result = true;

                switch (_coverDatasFlags[coverData])
                {
                    case DataStateFlag.Created:
                        result = await _persistence.CreateCoverAsync(coverData);
                        break;
                    case DataStateFlag.Updated:
                        break;
                    case DataStateFlag.Deleted:
                        break;
                }

                if (!result)
                {
                    throw new InvalidOperationException("Operation " + _coverDatasFlags[coverData] + " failed on cover " + coverData.Id);
                }

                _coverDatasFlags.Remove(coverData);
            }
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
        private async Task SaveBookAuthorAsync()
        {
            List<BookAuthorData> bookAuthorsToSave = _bookAuthorDatasFlags.Keys.ToList();

            foreach (BookAuthorData bookAuthorData in bookAuthorsToSave)
            {
                Boolean result = true;

                switch (_bookAuthorDatasFlags[bookAuthorData])
                {
                    case DataStateFlag.Created:
                        result = await _persistence.CreateBookAuthorAsync(bookAuthorData);
                        break;
                    case DataStateFlag.Updated:
                        break;
                    case DataStateFlag.Deleted:
                        result = await _persistence.DeleteBookAuthorAsync(bookAuthorData);
                        break;
                }

                if (!result)
                {
                    throw new InvalidOperationException("Operation " + _bookAuthorDatasFlags[bookAuthorData] + " failed on bookAuthor " + bookAuthorData.Id);
                }

                _bookAuthorDatasFlags.Remove(bookAuthorData);
            }
        }
        private async Task SaveVolumeAsync()
        {
            List<VolumeData> volumesToSave = _volumeDatasFlags.Keys.ToList();

            foreach (VolumeData volumeData in volumesToSave)
            {
                Boolean result = true;

                switch (_volumeDatasFlags[volumeData])
                {
                    case DataStateFlag.Created:
                        result = await _persistence.CreateVolumeAsync(volumeData);
                        break;
                    case DataStateFlag.Updated:
                        result = await _persistence.UpdateVolumeAsync(volumeData);
                        break;
                    case DataStateFlag.Deleted:
                        result = await _persistence.DeleteVolumeAsync(volumeData);
                        break;
                }

                if (!result)
                {
                    throw new InvalidOperationException("Operation " + _volumeDatasFlags[volumeData] + " failed on volume " + volumeData.Id);
                }

                _volumeDatasFlags.Remove(volumeData);
            }
        }
        private async Task SaveLendingAsync()
        {
            List<LendingData> lendingsToSave = _lendingDatasFlags.Keys.ToList();

            foreach (LendingData lendingData in lendingsToSave)
            {
                Boolean result = true;

                switch (_lendingDatasFlags[lendingData])
                {
                    case DataStateFlag.Created:
                        break;
                    case DataStateFlag.Updated:
                        result = await _persistence.UpdateLendingAsync(lendingData);
                        break;
                    case DataStateFlag.Deleted:
                        result = await _persistence.DeleteLendingAsync(lendingData);
                        break;
                }

                if (!result)
                {
                    throw new InvalidOperationException("Operation " + _lendingDatasFlags[lendingData] + " failed on lending " + lendingData.Id);
                }

                _lendingDatasFlags.Remove(lendingData);
            }
        }
    }
}
