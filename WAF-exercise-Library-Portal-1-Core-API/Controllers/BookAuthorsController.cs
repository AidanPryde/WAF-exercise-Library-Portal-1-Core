using System;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WAF_exercise_Library_Portal_1_Core_Db;
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

        // GET: api/BookAuthors/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/BookAuthors
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/BookAuthors/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/BookAuthors/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
