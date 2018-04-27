using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WAF_exercise_Library_Portal_1_Core_Db;

namespace WAF_exercise_Library_Portal_1_Core_WA.Services
{
    public interface ILibraryService
    {
        IEnumerable<Book> Books { get; }

        Byte[] GetBookCoverImage(Int32? bookId);
    }
}
