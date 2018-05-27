using System;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

using WAF_exercise_Library_Portal_1_Core_Db;
using WAF_exercise_Library_Portal_1_Core_Db.Models;
using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;

namespace WAF_exercise_Library_Portal_1_Core_API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/Books
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_context
                        .Book
                        .ToList()
                        .Select(b => new BookData(b.Id,
                            b.Title,
                            b.PublishedYear,
                            b.Isbn,
                            b.CoverId == null ? null : new CoverData(b.CoverId ?? throw new Exception())
                        )));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_context
                        .Book
                        .Where(b => b.Id == id)
                        .ToList()
                        .Select(b => new BookData(b.Id,
                            b.Title,
                            b.PublishedYear,
                            b.Isbn,
                            new CoverData(b.Id,
                                b.Cover?.Image) 
                        ))
                        .Single());
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        // POST: api/Books
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Post([FromBody] BookData bookData)
        {
            try
            {
                var addedBook = _context.Book.Add(new Book
                {
                    Title = bookData.Title,
                    PublishedYear = bookData.PublishedYear,
                    Isbn = bookData.Isbn,
                });

                _context.SaveChanges();

                bookData.Id = addedBook.Entity.Id;

                return Ok(bookData.Id);

            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        // PUT: api/Books
        [HttpPut]
        [Authorize(Roles = "admin")]
        public IActionResult Put([FromBody] BookData bookData)
        {
            try
            {
                Book book = _context.Book.FirstOrDefault(b => b.Equals(bookData.Id));

                if (book == null)
                {
                    return NotFound();
                }

                book.Title = bookData.Title;
                book.PublishedYear = bookData.PublishedYear;
                book.Isbn = bookData.Isbn;

                _context.SaveChanges();

                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(Int32 id)
        {
            try
            {
                Book book = _context.Book.FirstOrDefault(b => b.Equals(id));

                if (book == null)
                {
                    return NotFound();
                }

                _context.Book.Remove(book);

                _context.SaveChanges();

                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
