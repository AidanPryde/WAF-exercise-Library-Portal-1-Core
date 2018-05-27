using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;

namespace WAF_exercise_Library_Portal_1_Core_WPF.Models
{
    public interface ILibraryModel
    {
        Boolean IsUserLoggedIn { get; }

        Task<Boolean> LoginAsync(String name, String password);
        Task<Boolean> LogoutAsync();

        IReadOnlyList<BookData> Books { get; }
        void CreateBook(BookData bookData);
        void UpdateBook(BookData bookData);
        void DeleteBook(BookData bookData);
        event EventHandler<Int32> BookChanged;

        IReadOnlyList<BookAuthorData> BookAuthors { get; }
        void AddBookAuthor(BookAuthorData bookAuthorData);
        void RemoveBookAuthor(BookAuthorData bookAuthorData);
        event EventHandler<Int32> BookAuthorChanged;

        IReadOnlyList<AuthorData> Authors { get; }
        void AddAuthor(AuthorData authorData, Int32 bookId);
        void UpdateAuthor(AuthorData authorData, Int32 bookId);
        void RemoveAuthor(AuthorData authorData, Int32 bookId);
        event EventHandler<Int32> AuthorChanged;

        IReadOnlyList<CoverData> Covers { get; }
        void AddCover(CoverData authorData);
        void RemoveCover(CoverData authorData);
        event EventHandler<Int32> CoverChanged;

        IReadOnlyList<VolumeData> Volumes { get; }
        void AddVolume(VolumeData authorData);
        void UpdateVolume(VolumeData authorData);
        void DeleteVolume(VolumeData authorData);
        void SortingOutVolume(VolumeData authorData);
        event EventHandler<Int32> VolumeChanged;

        IReadOnlyList<LendingData> Lendings { get; }
        event EventHandler<Int32> LendingChanged;

        Task LoadAsync();
        Task SaveAsync();
    }
}
