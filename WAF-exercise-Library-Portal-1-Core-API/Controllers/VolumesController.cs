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
    public class VolumesController : Controller
    {
        private readonly LibraryDbContext _context;

        public VolumesController(LibraryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/Volumes
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_context
                        .Volume
                        .ToList()
                        .Select(v => new VolumeData(v.Id,
                            v.IsSordtedOut,
                            new BookData(v.BookId))));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        // GET: api/Volumes/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Volumes
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Volumes/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Volumes/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET: api/Volumes/BookId/5
        [HttpGet("BookId/{id}")]
        public IActionResult GetWhereBookId(Int32 id)
        {
            try
            {
                return Ok(_context
                        .Volume
                        .Where(v => v.BookId == id)
                        .ToList()
                        .Select(v => new VolumeData(v.Id,
                            v.IsSordtedOut,
                            new BookData(v.BookId)
                        )));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
