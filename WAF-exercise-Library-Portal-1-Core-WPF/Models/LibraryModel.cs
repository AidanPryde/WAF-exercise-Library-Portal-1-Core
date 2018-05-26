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
            Updated,
            Deleted
        }

        private ILibraryPersistence _persistence;

        private List<BookData> _books;
        private Dictionary<BookData, DataStateFlag> _booksFlags;

        public IReadOnlyList<BookData> Books
        {
            get { return _books; }
        }

        public Boolean IsUserLoggedIn { get; private set; }

        public LibraryModel(ILibraryPersistence persistence)
        {
            IsUserLoggedIn = false;
            _persistence = persistence;
        }

        public event EventHandler<Int32> BookChanged;

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

        private void OnBuildingChanged(Int32 buildingId)
        {
            BookChanged?.Invoke(this, buildingId);
        }

        public void CreateBook(BookData bookData)
        {
            if (bookData == null)
            {
                throw new ArgumentNullException(nameof(bookData));
            }

            if (_books.Contains(bookData))
            {
                throw new ArgumentException("The book is already in the collection.", nameof(bookData));
            }

            bookData.Id = (_books.Count > 0 ? _books.Max(b => b.Id) : 0) + 1;
            _booksFlags.Add(bookData, DataStateFlag.Created);
            _books.Add(bookData);

            OnBuildingChanged(bookData.Id);
        }

        public void UpdateBook(BookData bookData)
        {
            if (bookData == null)
            {
                throw new ArgumentNullException(nameof(bookData));
            }

            BookData bookToModify = _books.FirstOrDefault(b => b.Id == bookData.Id)
                ?? throw new ArgumentException("The book does not exist.", nameof(bookData));

            bookToModify.Title = bookData.Title;
            bookToModify.PublishedYear = bookData.PublishedYear;
            bookToModify.Isbn = bookData.Isbn;

            if (_booksFlags.ContainsKey(bookToModify)
             && _booksFlags[bookToModify] == DataStateFlag.Created)
            {
                _booksFlags[bookToModify] = DataStateFlag.Created;
            }
            else
            {
                _booksFlags[bookToModify] = DataStateFlag.Updated;
            }

            OnBuildingChanged(bookData.Id);
        }

        public void DeleteBook(int bookId)
        {
            throw new NotImplementedException();
        }

        public async Task LoadAsync()
        {
            _books = (await _persistence.ReadBooksAsync()).ToList();

            _booksFlags = new Dictionary<BookData, DataStateFlag>();
        }

        public async Task SaveAsync()
        {
            List<BookData> booksToSave = _booksFlags.Keys.ToList();

            foreach (BookData bookData in booksToSave)
            {
                Boolean result = true;

                // az állapotjelzőnek megfelelő műveletet végezzük el
                switch (_booksFlags[bookData])
                {
                    case DataStateFlag.Created:
                        result = await _persistence.CreateBuildingAsync(bookData);
                        break;
                    case DataStateFlag.Deleted:
                        result = await _persistence.DeleteBuildingAsync(bookData);
                        break;
                    case DataStateFlag.Updated:
                        result = await _persistence.UpdateBuildingAsync(bookData);
                        break;
                }

                if (!result)
                {
                    throw new InvalidOperationException("Operation " + _booksFlags[bookData] + " failed on building " + bookData.Id);
                }

                _booksFlags.Remove(bookData);
            }
        }
    }
}
