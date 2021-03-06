﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using WAF_exercise_Library_Portal_1_Core_Db;
using WAF_exercise_Library_Portal_1_Core_Db.Models;
using WAF_exercise_Library_Portal_1_Core_WA.Models;

namespace WAF_exercise_Library_Portal_1_Core_WA.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly LibraryDbContext _context;

        public IEnumerable<Book> BooksWithAll
        {
            get
            {
                return _context.Book
                    .Include(b => b.BookAuthors)
                        .ThenInclude(ba => ba.Author)
                    .Include(b => b.Volumes)
                        .ThenInclude(v => v.Lendings)
                        .ThenInclude(l => l.ApplicationUser);
            }
        }
        public IEnumerable<Book> BooksWithAuthorsVolumesLendings
        {
            get
            {
                return _context.Book
                    .Include(b => b.BookAuthors)
                        .ThenInclude(ba => ba.Author)
                    .Include(b => b.Volumes)
                        .ThenInclude(v => v.Lendings);
            }
        }
        public IEnumerable<Book> BooksWithAuthors
        {
            get
            {
                return _context.Book.Include(b => b.BookAuthors).ThenInclude(ba => ba.Author);
            }
        }
        private IEnumerable<Volume> Volumes
        {
            get
            {
                return _context.Volume;
            }
        }

        public LibraryService(LibraryDbContext context)
        {
            _context = context;
        }

        public Byte[] GetBookCoverImage(Int32? bookId)
        {
            try
            {
                if (bookId == null)
                {
                    return null;
                }

                Book book = _context.Book.FirstOrDefault(b => bookId == b.Id);

                if (book == null)
                {
                    return null;
                }

                Int32? coderId = book.CoverId;

                if (coderId == null)
                {
                    return null;
                }

                Cover cover = _context.Cover.FirstOrDefault(c => coderId == c.Id);

                if (cover == null)
                {
                    return null;
                }

                Byte[] imageContent = cover.Image;

                if (imageContent == null)
                {
                    return null;
                }

                return imageContent;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<Book> NarrowBooksByAuthorAndTitle(String author, String title)
        {
            try
            {
                if (String.IsNullOrEmpty(author) == false && String.IsNullOrEmpty(title) == false)
                {
                    author = author.ToLower();
                    title = title.ToLower();

                    return BooksWithAuthors.Where(b => (
                        (b.BookAuthors.Where(ba => ba.Author.Name.ToLower().Contains(author)).Any())
                     && (b.Title.ToLower().Contains(title))
                    ));
                }

                if (String.IsNullOrEmpty(title) == false)
                {
                    title = title.ToLower();

                    return BooksWithAuthors.Where(b => b.Title.ToLower().Contains(title));
                }

                if (String.IsNullOrEmpty(author) == false)
                {
                    author = author.ToLower();

                    return BooksWithAuthors.Where(b => (b.BookAuthors.Any(ba => ba.Author.Name.ToLower().Contains(author))));
                }

                return BooksWithAuthors;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IEnumerable<Book> NarrowBooksSelection(IEnumerable<Book> books, Int32 from, Int32 pagingSzie = 20)
        {
            try
            {
                return books.Skip(from).Take(pagingSzie);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Book GetBookByBookId(Int32 id)
        {
            try
            {
                return BooksWithAll.FirstOrDefault(b => b.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Int32? GetBookIdByVolumeId(String id)
        {
            try
            {
                Volume volume = GetVolumeByVolumeId(id);
                if (volume == null)
                {
                    return null;
                }

                return volume.BookId;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<Int32?> GetBookIdByLendingId(Int32 id)
        {
            try
            {
                Lending lending = await _context.Lending.Where(l => l.Id == id).Include(l => l.Volume).FirstOrDefaultAsync();

                if (lending == null)
                {
                    return null;
                }

                return lending.Volume.BookId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Volume GetVolumeByVolumeId(String id)
        {
            try
            {
                return Volumes.FirstOrDefault(v => v.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<UpdateResult> SaveLending(Int32 applicationUserId, LendingViewModel lendingViewModel)
        {
            try
            {
                Lending lending = new Lending
                {
                    Active = 0,
                    ApplicationUserId = applicationUserId,
                    StartDate = lendingViewModel.StartDate,
                    EndDate = lendingViewModel.EndDate,
                    VolumeId = lendingViewModel.VolumeId
                };

                Volume volume = await _context.Volume.Where(v => v.Id == lending.VolumeId).Include(v => v.Lendings).FirstOrDefaultAsync();

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

                await _context.Lending.AddAsync(lending);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return UpdateResult.Success;
                }

                return UpdateResult.DbError;
            }
            catch (Exception)
            {
                return UpdateResult.DbError;
            }
            
        }
        public async Task<UpdateResult> RemoveLending(Int32 lendingId, Int32 applicationUserId)
        {
            try
            {
                Lending lending = await _context.Lending.FirstOrDefaultAsync(l => l.Id == lendingId && l.ApplicationUserId == applicationUserId);

                if (lending == null)
                {
                    return UpdateResult.DbError;
                }

                _context.Lending.Remove(lending);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return UpdateResult.Success;
                }

                return UpdateResult.DbError;
            }
            catch (Exception)
            {
                return UpdateResult.DbError;
            }
        }
    }
}
