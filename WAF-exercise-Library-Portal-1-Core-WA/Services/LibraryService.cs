using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using WAF_exercise_Library_Portal_1_Core_Db;
using WAF_exercise_Library_Portal_1_Core_WA.Models;

namespace WAF_exercise_Library_Portal_1_Core_WA.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly LibraryDbContext _context;

        public IEnumerable<Book> Books => _context.Book.Include(b => b.BookAuthors).ThenInclude(ba => ba.Author).Include(b => b.Volumes).ThenInclude(v => v.Lendings).ThenInclude(l => l.ApplicationUser);
        public IEnumerable<Volume> Volumes => _context.Volume.Include(v => v.Lendings);

        public LibraryService(LibraryDbContext context)
        {
            _context = context;
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

        public IEnumerable<Book> NarrowBooksByAuthorAndTitle(String author, String title)
        {
            if (String.IsNullOrEmpty(author) == false && String.IsNullOrEmpty(title) == false)
            {
                author = author.ToLower();
                title = title.ToLower();

                return Books.Where(b => (
                    (b.BookAuthors.Where(ba => ba.Author.Name.ToLower().Contains(author)).Any())
                 || (b.Title.ToLower().Contains(title))
                ));
            }

            if (String.IsNullOrEmpty(title) == false)
            {
                title = title.ToLower();

                return Books.Where(b => b.Title.ToLower().Contains(title));
            }

            if (String.IsNullOrEmpty(author) == false)
            {
                author = author.ToLower();

                return Books.Where(b => (b.BookAuthors.Where(ba => ba.Author.Name.ToLower().Contains(author)).Any()));
            }

            return Books;
        }

        public IEnumerable<Book> NarrowBooksSelection(IEnumerable<Book> books, Int32 from, Int32 pagingSzie = 20)
        {
            return books.Skip(from).Take(pagingSzie);
        }

        public Book GetBookByBookId(Int32 id)
        {
            return Books.Where(b => b.Id == id).FirstOrDefault();
        }

        public Int32 GetBookIdByVolumeId(String id)
        {
            return GetVolumeByVolumeId(id).BookId;
        }

        public Int32 GetBookIdByLendingId(Int32 id)
        {
            return _context.Lending.Where(l => l.Id == id).Include(l => l.Volume).FirstOrDefault().Volume.BookId;
        }

        public Volume GetVolumeByVolumeId(String id)
        {
            return Volumes.Where(v => v.Id == id).FirstOrDefault();
        }

        public UpdateResult SaveLending(Int32 applicationUserId, LendingViewModel lendingViewModel)
        {
            Lending lending = new Lending
            {
                Active = 0,
                ApplicationUserId = applicationUserId,
                StartDate = lendingViewModel.StartDate,
                EndDate = lendingViewModel.EndDate,
                VolumeId = lendingViewModel.VolumeId
            };

            Volume volume = _context.Volume.Where(v => v.Id == lending.VolumeId).Include(v => v.Lendings).First();
            if (volume == null)
            {
                return UpdateResult.DbError;
            }

            if (lending.StartDate < DateTime.UtcNow
             || lending.EndDate < DateTime.UtcNow
             || lending.StartDate > lending.EndDate
             || (lending.EndDate - lending.StartDate).Days < 1)
            {
                return UpdateResult.ConcurrencyError;
            }

            foreach (Lending otherlending in volume.Lendings)
            {
                if (otherlending.StartDate > lending.EndDate
                 || otherlending.EndDate < lending.StartDate)
                {

                }
                else
                {
                    return UpdateResult.ConcurrencyError;
                }
            }

            _context.Lending.Add(lending);

            if (_context.SaveChanges() > 0)
                return UpdateResult.Success;
            return UpdateResult.DbError;
        }

        public UpdateResult RemoveLending(Int32 lendingId, Int32 applicationUserId)
        {
            Lending lending = _context.Lending.Where(l => l.Id == lendingId && l.ApplicationUserId == applicationUserId).FirstOrDefault();

            _context.Lending.Remove(lending);

            if (_context.SaveChanges() > 0)
                return UpdateResult.Success;
            return UpdateResult.DbError;
        }
    }
}
