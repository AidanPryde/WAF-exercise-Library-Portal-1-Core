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
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
        private readonly LibraryDbContext _context;

        public AuthorsController(LibraryDbContext context)
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
                        .Author
                        .ToList()
                        .Select(a => new AuthorData(a.Id,
                            a.Name
                        )));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public IActionResult Get(Int32 id)
        {
            try
            {
                return Ok(_context
                    .Author
                    .Where(a => a.Id == id)
                    .Select(a => new AuthorData
                {
                    Id = a.Id,
                    Name = a.Name
                }).Single());
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        // POST: api/Authors
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        
        // GET: api/Authors/BookId/5
        [HttpGet("BookId/{id}")]
        public IActionResult GetWhereBookId(Int32 id)
        {
            try
            {
                return Ok(_context
                    .BookAuthor
                    .Include(a => a.Author)
                    .Where(a => a.BookId == id)
                    .Select(a => new AuthorData
                    {
                        Id = a.Author.Id,
                        Name = a.Author.Name
                    }));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
