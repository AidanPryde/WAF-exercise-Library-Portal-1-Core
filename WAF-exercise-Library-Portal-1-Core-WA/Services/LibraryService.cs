using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

using WAF_exercise_Library_Portal_1_Core_Db;

namespace WAF_exercise_Library_Portal_1_Core_WA.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly LibraryDbContext _context;

        public IEnumerable<Book> Books => _context.Book.Include(b => b.BookAuthors).ThenInclude(ba => ba.Author);

        public LibraryService(LibraryDbContext context)
        {
            _context = context;
        }

        public enum UpdateResult
        {
            Success,
            ConcurrencyError,
            DbError
        }

        public Byte[] GetBookCoverImage(Int32? bookId)
        {
            if (bookId == null)
                return null;

            Int32? coderId = _context.Book.Where(b => bookId == b.Id).FirstOrDefault().CoverId;

            if (coderId == null)
                return null;

            Byte[] imageContent = _context.Cover.Where(c => coderId == c.Id).FirstOrDefault().Image;

            if (imageContent == null)
                return null;

            return imageContent;
        }
    }
}
