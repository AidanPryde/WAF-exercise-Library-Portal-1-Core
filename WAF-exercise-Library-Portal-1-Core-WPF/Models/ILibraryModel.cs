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

        IReadOnlyList<BookData> BookDatas { get; }
        void CreateBook(BookData bookData);
        void UpdateBook(BookData bookData);
        void DeleteBook(BookData bookData);
        event EventHandler<Int32> BookDatasChanged;

        IReadOnlyList<BookAuthorData> BookAuthorDatas { get; }
        void CreateBookAuthor(BookAuthorData bookAuthorData);
        void DeleteBookAuthor(Int32 bookAuthorId);
        event EventHandler<Int32> BookAuthorDatasChanged;

        IReadOnlyList<AuthorData> AuthorDatas { get; }
        void AddAuthor(Int32 authorId, Int32 bookId);
        void CreateAuthor(AuthorData authorData, Int32 bookId);
        void UpdateAuthor(AuthorData authorData);
        void RemoveAuthor(Int32 authorId, Int32 bookId);
        event EventHandler<Int32> AuthorDatasChanged;

        IReadOnlyList<CoverData> CoverDatas { get; }
        CoverData CreateCover(Int32 bookId, Byte[] image);
        CoverData AddCover(Int32 bookId, Int32 coverId);
        void RemoveCover(Int32 bookId);
        event EventHandler<Int32> CoverDatasChanged;

        IReadOnlyList<VolumeData> VolumeDatas { get; }
        void CreateVolume(VolumeData volumeData, Int32 bookId);
        void UpdateVolume(VolumeData volumeData);
        void DeleteVolume(String volumeDataId);
        event EventHandler<String> VolumeDatasChanged;

        IReadOnlyList<LendingData> LendingDatas { get; }
        event EventHandler<Int32> LendingDatasChanged;

        Task LoadAsync();
        Task SaveAsync();
    }
}
