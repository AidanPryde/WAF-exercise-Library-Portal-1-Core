using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        void DeleteBook(Int32 bookId);

        event EventHandler<Int32> BookChanged;

        Task LoadAsync();
        Task SaveAsync();
    }
}
