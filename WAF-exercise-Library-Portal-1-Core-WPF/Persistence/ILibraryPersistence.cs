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
        Task<Boolean> CreateBookAuthorAsync(BookAuthorData bookAuthorData);
        Task<Boolean> DeleteBookAuthorAsync(BookAuthorData bookAuthorData);

        Task<IEnumerable<AuthorData>> ReadAuthorsAsync();
        Task<Boolean> CreateAuthorAsync(AuthorData authorData);
        Task<Boolean> UpdateAuthorAsync(AuthorData authorData);

        Task<IEnumerable<CoverData>> ReadCoversAsync();
        Task<Boolean> CreateCoverAsync(CoverData coverData);
        Task<Boolean> DeleteCoverAsync(CoverData coverData);

        Task<IEnumerable<VolumeData>> ReadVolumesAsync();
        Task<Boolean> CreateVolumeAsync(VolumeData volumeData);
        Task<Boolean> UpdateVolumeAsync(VolumeData volumeData);
        Task<Boolean> DeleteVolumeAsync(VolumeData volumeData);

        Task<IEnumerable<LendingData>> ReadLendingsAsync();
        Task<Boolean> UpdateLendingAsync(LendingData volumeData);
        Task<Boolean> DeleteLendingAsync(LendingData volumeData);

        Task<Boolean> LoginAsAdminAsync(LoginData user);
        Task<Boolean> LogoutAsync();
        
    }
}
