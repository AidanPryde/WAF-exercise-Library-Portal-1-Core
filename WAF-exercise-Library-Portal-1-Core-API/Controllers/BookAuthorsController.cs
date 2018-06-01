using System;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WAF_exercise_Library_Portal_1_Core_Db;
using WAF_exercise_Library_Portal_1_Core_Db.Models;
using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;

namespace WAF_exercise_Library_Portal_1_Core_API.Controllers
{
    [Produces("application/json")]
    [Route("api/BookAuthors")]
    public class BookAuthorsController : Controller
    {
        private readonly LibraryDbContext _context;

        public BookAuthorsController(LibraryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/Authors
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_context
                        .BookAuthor
                        .ToList()
                        .Select(ba => new BookAuthorData(ba.Id,
                            new BookData(ba.BookId),
                            new AuthorData(ba.AuthorId))));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Authors/{id}
        [HttpGet("{id}", Name = "GetBookAuthor")]
        public IActionResult GetBookAuthor(Int32 id)
        {
            try
            {
                return Ok(_context
                        .BookAuthor
                        .Where(ba => ba.Id == id)
                        .Select(ba => new BookAuthorData(ba.Id,
                            new BookData(ba.BookId),
                            new AuthorData(ba.AuthorId)))
                        .Single());
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/BookAuthors
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Post([FromBody] BookAuthorData bookAuthorData)
        {
            try
            {
                if (bookAuthorData.BookData == null || bookAuthorData.AuthorData == null)
                {
                    return NoContent();
                }

                var addedBookAuthor = _context.BookAuthor.Add(new BookAuthor
                {
                    BookId = bookAuthorData.BookData.Id,
                    AuthorId = bookAuthorData.AuthorData.Id
                });

                _context.SaveChanges();

                return Ok(addedBookAuthor.Entity.Id);
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/BookAuthors/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(Int32 id)
        {
            try
            {
                BookAuthor bookAuthor = _context.BookAuthor.FirstOrDefault(ba => ba.Id == id);

                if (bookAuthor == null)
                {
                    return NotFound();
                }

                _context.BookAuthor.Remove(bookAuthor);
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
