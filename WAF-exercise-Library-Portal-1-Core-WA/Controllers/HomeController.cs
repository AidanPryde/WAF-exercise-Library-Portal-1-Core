using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WAF_exercise_Library_Portal_1_Core_Db.Models;
using WAF_exercise_Library_Portal_1_Core_WA.Models;
using WAF_exercise_Library_Portal_1_Core_WA.Services;

namespace WAF_exercise_Library_Portal_1_Core_WA.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ILibraryService libraryService) : base(libraryService)
        {
        }

        [HttpGet]
        public IActionResult Index(Int32? paging, Boolean? order, SearchViewModel searchViewModel)
        {
            IEnumerable<Book> books = _libraryService.NarrowBooksByAuthorAndTitle(searchViewModel.Author, searchViewModel.Title);

            Int32 booksCount = books.Count(b => true);
            Int32 currentPaging = paging ?? 0;
            Boolean currentOrder = order ?? false;
            Int32 pagingSize = 20;

            if (currentPaging * pagingSize > booksCount - 1)
            {
                currentPaging = (booksCount / pagingSize);
            }

            List<Int32[]> pagingTab = CalculatePaging(booksCount);

            if (currentOrder)
            {
                books = books.OrderBy(b => b.Title.ToLower());
            }
            else
            {
                books = books.OrderByDescending(b => b.CountValidLendings());
            }

            books = _libraryService.NarrowBooksSelection(books, currentPaging * pagingSize, pagingSize);

            ViewBag.PagingTab = pagingTab;
            ViewBag.Order = currentOrder;

            ViewBag.Books = books.ToList();

            return View("Index", searchViewModel);
        }

        public IActionResult Details(Int32? bookId)
        {
            Int32 currentBookId = bookId ?? -1;
            if (currentBookId == -1)
                return RedirectToAction(nameof(HomeController.Index));

            Book book = _libraryService.GetBookByBookId(currentBookId);

            if (book == null)
                return RedirectToAction(nameof(HomeController.Index));

            return View("Details", book);
        }

        public FileResult ImageForBookCover(Int32? bookId)
        {
            Byte[] imageContent = _libraryService.GetBookCoverImage(bookId);

            if (imageContent == null)
                return File("~/images/NoImage.png", "image/png");

            return File(imageContent, "image/png");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Int32[]> CalculatePaging(Int32 total, Int32 pageSize = 20)
        {
            Int32 from = 1;
            Int32 to = total > pageSize ? pageSize : total;

            List<Int32[]> pagingTab = new List<Int32[]>();

            if (total == 0)
            {
                pagingTab.Add(new Int32[] { 0, 0 });

                return pagingTab;
            }

            while (to < total)
            {
                pagingTab.Add(new Int32[] { from, to });
                from = to + 1;
                to += pageSize;
            }

            if (to > total)
            {
                to = total;
            }

            pagingTab.Add(new Int32[] { from, to });

            return pagingTab;
        }
    }
}
