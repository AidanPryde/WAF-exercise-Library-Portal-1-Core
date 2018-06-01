using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;

namespace WAF_exercise_Library_Portal_1_Core_WPF.Models
{
    public interface ILibraryModel
    {
        Boolean IsUserLoggedIn { get; }

        Task<Boolean> LoginAsAdminAsync(String name, String password);
        Task<Boolean> LogoutAsync();

        IReadOnlyList<BookData> BookDatas { get; }
        void CreateBook(BookData bookData);
        void UpdateBook(BookData bookData);
        void DeleteBook(BookData bookData);
        event EventHandler<BookData> BookDataChanged;
        event EventHandler<BookData> BookDataDeleted;

        IReadOnlyList<BookAuthorData> BookAuthorDatas { get; }
        void CreateBookAuthor(BookAuthorData bookAuthorData);
        void DeleteBookAuthor(Int32 bookAuthorId);
        event EventHandler<BookAuthorData> BookAuthorDataCreated;
        event EventHandler<BookAuthorData> BookAuthorDataDeleted;

        IReadOnlyList<AuthorData> AuthorDatas { get; }
        void AddAuthor(Int32 authorId, Int32 bookId);
        void CreateAuthor(AuthorData authorData, Int32 bookId);
        void UpdateAuthor(AuthorData authorData);
        void RemoveAuthor(Int32 authorId, Int32 bookId);
        event EventHandler<AuthorData> AuthorDataChanged;

        IReadOnlyList<CoverData> CoverDatas { get; }
        void CreateCover(Int32 bookId, Byte[] image);
        void AddCover(Int32 bookId, Int32 coverId);
        void RemoveCover(Int32 bookId);
        event EventHandler<BookData> CoverDataChanged;
        event EventHandler<BookData> CoverDataRemoved;

          IReadOnlyList<VolumeData> VolumeDatas { get; }
        void CreateVolume(VolumeData volumeData, Int32 bookId);
        void UpdateVolume(VolumeData volumeData);
        void DeleteVolume(String volumeDataId);
        event EventHandler<VolumeData> VolumeDataChanged;
        event EventHandler<VolumeData> VolumeDataDeleted;

        IReadOnlyList<LendingData> LendingDatas { get; }
        void TurnLending(LendingData lendingData);
        void DeleteLending(Int32 lendingData);
        event EventHandler<LendingData> LendingDataChanged;
        event EventHandler<LendingData> LendingDataDeleted;

        Task LoadAsync();
        Task SaveAsync();
    }
}
