using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;

namespace WAF_exercise_Library_Portal_1_Core_WPF.Persistence
{
    public interface ILibraryPersistence
    {
        Task<IEnumerable<BookData>> ReadBooksAsync();

        Task<Boolean> CreateBuildingAsync(BookData bookData);
        Task<Boolean> UpdateBuildingAsync(BookData bookData);
        Task<Boolean> DeleteBuildingAsync(BookData bookData);

        Task<Boolean> LoginAsync(LoginData user);
        Task<Boolean> LogoutAsync();
    }
}
