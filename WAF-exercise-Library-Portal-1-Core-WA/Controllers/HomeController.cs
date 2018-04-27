using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WAF_exercise_Library_Portal_1_Core_WA.Models;
using WAF_exercise_Library_Portal_1_Core_WA.Services;

namespace WAF_exercise_Library_Portal_1_Core_WA.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ILibraryService libraryService) : base(libraryService)
        {
        }

        public IActionResult Index(Int32? paging, Boolean? order)
        {
            Int32 currentPaging = paging ?? 0;

            Int32 pageSize = 20;
            Int32 total = _libraryService.Books.Count();

            Int32 from = 1;
            Int32 to = total > pageSize ? 20 : total;

            List<Int32[]> pagingTab = new List<Int32[]>();
                
            while (to < total)
            {
                pagingTab.Add(new Int32[] { from, to });
                from = to + 1;
                to += pageSize;
            }

            if (to < total)
            {
                to = total;
            }

            pagingTab.Add(new Int32[] { from, to });

            ViewBag.PagingTab = pagingTab;

            return View("Index", _libraryService.Books.Skip(pagingTab[currentPaging][0] - 1).Take(pageSize).ToList());
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
    }
}
