using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;

namespace WAF_exercise_Library_Portal_1_Core_WPF.Persistence
{
    public interface ILibraryPersistence
    {
        Task<IEnumerable<BookData>> ReadBooksAsync();
        Task<Boolean> CreateBookAsync(BookData bookData);
        Task<Boolean> UpdateBookAsync(BookData bookData);
        Task<Boolean> DeleteBookAsync(BookData bookData);

        Task<IEnumerable<BookAuthorData>> ReadBookAuthorsAsync();
        Task<Boolean> AddBookAuthorAsync(BookAuthorData bookAuthorData);
        Task<Boolean> RemoveBookAuthorAsync(BookAuthorData bookAuthorData);

        Task<IEnumerable<AuthorData>> ReadAuthorsAsync();
        Task<Boolean> AddAuthorAsync(AuthorData authorData);
        Task<Boolean> UpdateAuthorAsync(AuthorData authorData);
        Task<Boolean> RemoveAuthorAsync(AuthorData authorData);

        Task<IEnumerable<CoverData>> ReadCoversAsync();
        Task<Boolean> AddCoverAsync(CoverData coverData);
        Task<Boolean> DeleteCoverAsync(CoverData coverData);

        Task<IEnumerable<VolumeData>> ReadVolumesAsync();
        Task<Boolean> AddVolumeAsync(VolumeData volumeData);
        Task<Boolean> UpdateVolumeAsync(VolumeData volumeData);
        Task<Boolean> RemoveVolumeAsync(VolumeData volumeData);
        Task<Boolean> SortOutVolumeAsync(VolumeData volumeData);

        Task<IEnumerable<LendingData>> ReadLendingsAsync();
        Task<Boolean> AddLendingAsync(LendingData volumeData);
        Task<Boolean> UpdateLendingAsync(LendingData volumeData);
        Task<Boolean> RemoveLendingAsync(LendingData volumeData);

        Task<Boolean> LoginAsync(LoginData user);
        Task<Boolean> LogoutAsync();
        
    }
}
