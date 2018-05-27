using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public IReadOnlyList<BookData> Books { get { return _bookDatas; } }
        private Dictionary<BookData, DataStateFlag> _booksFlags;
        public event EventHandler<Int32> BookChanged;

        private List<BookAuthorData> _bookAuthorDatas;
        public IReadOnlyList<BookAuthorData> BookAuthors { get { return _bookAuthorDatas; } }
        private Dictionary<BookAuthorData, DataStateFlag> _bookAuthorsFlags;
        public event EventHandler<Int32> BookAuthorChanged;

        private List<AuthorData> _authorDatas;
        public IReadOnlyList<AuthorData> Authors { get { return _authorDatas; } }
        private Dictionary<AuthorData, DataStateFlag> _authorsFlags;
        public event EventHandler<Int32> AuthorChanged;

        private List<CoverData> _coverDatas;
        public IReadOnlyList<CoverData> Covers { get { return _coverDatas; } }
        private Dictionary<CoverData, DataStateFlag> _coversFlags;
        public event EventHandler<Int32> CoverChanged;

        private List<VolumeData> _volumeDatas;
        public IReadOnlyList<VolumeData> Volumes { get { return _volumeDatas; } }
        private Dictionary<VolumeData, DataStateFlag> _volumesFlags;
        public event EventHandler<Int32> VolumeChanged;

        private List<LendingData> _lendingDatas;
        public IReadOnlyList<LendingData> Lendings { get { return _lendingDatas; } }
        private Dictionary<LendingData, DataStateFlag> _lendingsFlags;
        public event EventHandler<Int32> LendingChanged;

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

        private void OnBookChanged(Int32 bookId)
        {
            BookChanged?.Invoke(this, bookId);
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
            _booksFlags.Add(bookData, DataStateFlag.Created);
            _bookDatas.Add(bookData);

            OnBookChanged(bookData.Id);
        }
        public void UpdateBook(BookData bookData)
        {
            if (bookData == null)
            {
                throw new ArgumentNullException(nameof(bookData));
            }

            BookData bookToModify = _bookDatas.FirstOrDefault(b => b.Equals(bookData.Id))
                ?? throw new ArgumentException("The book does not exist.", nameof(bookData));

            bookToModify.Title = bookData.Title;
            bookToModify.PublishedYear = bookData.PublishedYear;
            bookToModify.Isbn = bookData.Isbn;

            if (_booksFlags.ContainsKey(bookToModify) == false
             || _booksFlags[bookToModify] == DataStateFlag.Deleted)
            {
                _booksFlags[bookToModify] = DataStateFlag.Updated;
            }

            OnBookChanged(bookData.Id);
        }
        public void DeleteBook(BookData bookData)
        {
            if (bookData == null)
            {
                throw new ArgumentNullException(nameof(bookData));
            }

            BookData bookToDelete = _bookDatas.FirstOrDefault(b => b.Equals(bookData.Id))
                ?? throw new ArgumentException("The building does not exist.", nameof(bookData));

            if (_booksFlags.ContainsKey(bookToDelete)
             && _booksFlags[bookToDelete] == DataStateFlag.Created)
            {
                _booksFlags.Remove(bookToDelete);
            }
            else
            {
                _booksFlags[bookToDelete] = DataStateFlag.Deleted;
            }

            _bookDatas.Remove(bookToDelete);
        }

        private void OnBookAuthorChanged(Int32 bookAuthorId)
        {
            BookAuthorChanged?.Invoke(this, bookAuthorId);
        }
        public void AddBookAuthor(BookAuthorData bookAuthorData)
        {
            throw new NotImplementedException();
        }
        public void RemoveBookAuthor(BookAuthorData bookAuthorData)
        {
            throw new NotImplementedException();
        }

        private void OnAuthorChanged(Int32 authorId)
        {
            AuthorChanged?.Invoke(this, authorId);
        }
        public void AddAuthor(AuthorData authorData, Int32 bookId)
        {
            if (authorData == null)
            {
                throw new ArgumentNullException(nameof(authorData));
            }

            BookData bookData = _bookDatas.FirstOrDefault(b => b.Id == bookId);
            if (bookData == null)
            {
                throw new ArgumentException("No book in the collection.", nameof(bookId));
            }

            if (Authors.Contains(authorData))
            {
                throw new ArgumentException("Author already in the author collection.", nameof(bookId));
            }

            authorData.Id = (_authorDatas.Count > 0 ? _authorDatas.Max(b => b.Id) : 0) + 1;
            _authorDatas.Add(authorData);
            _authorsFlags.Add(authorData, DataStateFlag.Created);
            OnAuthorChanged(authorData.Id);

            //bookData.BookAuthors.Add(authorData);
            if (_booksFlags.ContainsKey(bookData) == false
             || _booksFlags[bookData] == DataStateFlag.Deleted)
            {
                _booksFlags[bookData] = DataStateFlag.Updated;
            }
            OnBookChanged(bookData.Id);
        }
        public void UpdateAuthor(AuthorData authorData, Int32 bookId)
        {
            if (authorData == null)
            {
                throw new ArgumentNullException(nameof(authorData));
            }

            BookData bookData = _bookDatas.FirstOrDefault(b => b.Id == bookId);
            if (bookData == null)
            {
                throw new ArgumentException("No book in the collection.", nameof(bookId));
            }

            /*if (bookData.BookAuthors.Contains(authorData) == false)
            {
                throw new ArgumentException("No author in the author collection of the book.", nameof(bookId));
            }*/

            AuthorData authorToModify = _authorDatas.FirstOrDefault(b => b.Equals(authorData))
                ?? throw new ArgumentException("The author does not exist.", nameof(authorData));

            authorToModify.Name = authorData.Name;

            if (_authorsFlags.ContainsKey(authorToModify) == false
             || _authorsFlags[authorToModify] == DataStateFlag.Deleted)
            {
                _authorsFlags[authorToModify] = DataStateFlag.Updated;
            }
            OnAuthorChanged(authorData.Id);

            //bookData.BookAuthors.First(bd => bd.Equals(authorData)).Name = authorData.Name;
            if (_booksFlags.ContainsKey(bookData) == false
             || _booksFlags[bookData] == DataStateFlag.Deleted)
            {
                _booksFlags[bookData] = DataStateFlag.Updated;
            }
            OnBookChanged(bookData.Id);
        }
        public void RemoveAuthor(AuthorData authorData, Int32 bookId)
        {
            if (authorData == null)
            {
                throw new ArgumentNullException(nameof(authorData));
            }

            BookData bookData = _bookDatas.FirstOrDefault(b => b.Id == bookId);
            if (bookData == null)
            {
                throw new ArgumentException("No book in the collection.", nameof(bookId));
            }

            /*if (bookData.BookAuthors.Contains(authorData) == false)
            {
                throw new ArgumentException("No author in the author collection of the book.", nameof(bookId));
            }*/

            AuthorData authorToRemove = _authorDatas.FirstOrDefault(b => b.Equals(authorData))
                ?? throw new ArgumentException("The author does not exist.", nameof(authorData));

            //bookData.BookAuthors.Remove(authorData);
            if (_booksFlags.ContainsKey(bookData) == false
             || _booksFlags[bookData] == DataStateFlag.Deleted)
            {
                _booksFlags[bookData] = DataStateFlag.Updated;
            }
            OnBookChanged(bookData.Id);
        }

        private void OnCoverChanged(Int32 coverId)
        {
            CoverChanged?.Invoke(this, coverId);
        }
        public void AddCover(CoverData authorData)
        {
            throw new NotImplementedException();
        }
        public void RemoveCover(CoverData authorData)
        {
            throw new NotImplementedException();
        }

        private void OnVolumeChanged(Int32 volumeId)
        {
            VolumeChanged?.Invoke(this, volumeId);
        }
        public void AddVolume(VolumeData authorData)
        {
            throw new NotImplementedException();
        }
        public void UpdateVolume(VolumeData authorData)
        {
            throw new NotImplementedException();
        }
        public void DeleteVolume(VolumeData authorData)
        {
            throw new NotImplementedException();
        }
        public void SortingOutVolume(VolumeData authorData)
        {
            throw new NotImplementedException();
        }

        private void OnLendingChanged(Int32 lendingId)
        {
            LendingChanged?.Invoke(this, lendingId);
        }

        public async Task LoadAsync()
        {
            _bookDatas = (await _persistence.ReadBooksAsync()).ToList();
            _booksFlags = new Dictionary<BookData, DataStateFlag>();

            _bookAuthorDatas = (await _persistence.ReadBookAuthorsAsync()).ToList();
            _bookAuthorsFlags = new Dictionary<BookAuthorData, DataStateFlag>();

            _authorDatas = (await _persistence.ReadAuthorsAsync()).ToList();
            _authorsFlags = new Dictionary<AuthorData, DataStateFlag>();

            _coverDatas = (await _persistence.ReadCoversAsync()).ToList();
            _coversFlags = new Dictionary<CoverData, DataStateFlag>();

            _volumeDatas = (await _persistence.ReadVolumesAsync()).ToList();
            _volumesFlags = new Dictionary<VolumeData, DataStateFlag>();

            _lendingDatas = (await _persistence.ReadLendingsAsync()).ToList();
            _lendingsFlags = new Dictionary<LendingData, DataStateFlag>();

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
                volumeData.Lendings = new List<LendingData>(_lendingDatas.Where(ld => ld.VolumeData.Equals(volumeData)));
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
            List<BookData> booksToSave = _booksFlags.Keys.ToList();

            foreach (BookData bookData in booksToSave)
            {
                Boolean result = true;

                switch (_booksFlags[bookData])
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
                        throw new InvalidOperationException("Operation " + _booksFlags[bookData] + " is invalid on book " + bookData.Id);
                }

                if (!result)
                {
                    throw new InvalidOperationException("Operation " + _booksFlags[bookData] + " failed on book " + bookData.Id);
                }

                _booksFlags.Remove(bookData);
            }
        }
        private async Task SaveAuthorsAsync()
        {
            List<AuthorData> authorsToSave = _authorsFlags.Keys.ToList();

            foreach (AuthorData authorData in authorsToSave)
            {
                Boolean result = true;

                switch (_authorsFlags[authorData])
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
                    throw new InvalidOperationException("Operation " + _authorsFlags[authorData] + " failed on author " + authorData.Id);
                }

                _authorsFlags.Remove(authorData);
            }
        }

        
    }
}
