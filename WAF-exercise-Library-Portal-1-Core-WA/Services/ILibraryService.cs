﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WAF_exercise_Library_Portal_1_Core_Db.Models;
using WAF_exercise_Library_Portal_1_Core_WA.Models;

namespace WAF_exercise_Library_Portal_1_Core_WA.Services
{
    public enum UpdateResult
    {
        Success,
        ConcurrencyError,
        DbError
    }

    public interface ILibraryService
    {
        IEnumerable<Book> BooksWithAll { get; }
        IEnumerable<Book> BooksWithAuthorsVolumesLendings { get; }
        IEnumerable<Book> BooksWithAuthors { get; }

        Byte[] GetBookCoverImage(Int32? bookId);

        IEnumerable<Book> NarrowBooksByAuthorAndTitle(String author, String title);
        IEnumerable<Book> NarrowBooksSelection(IEnumerable<Book> books, Int32 from, Int32 pagingSize = 20);
        Book GetBookByBookId(Int32 id);
        Int32? GetBookIdByVolumeId(String id);
        Task<Int32?> GetBookIdByLendingId(Int32 id);
        Volume GetVolumeByVolumeId(String id);

        Task<UpdateResult> SaveLending(Int32 applicationUserId, LendingViewModel lending);
        Task<UpdateResult> RemoveLending(Int32 lendingId, Int32 applicationUserId);
    }
}
