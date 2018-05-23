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

        Task<Boolean> LoginAsync(string name, string password);
        Task<Boolean> LogoutAsync();

        void CreateBook(BookData bookData);
        void UpdateBook(BookData bookData);
        void DeleteBook(Int32 bookId);

        Task LoadAsync();
        Task SaveAsync();
    }
}
