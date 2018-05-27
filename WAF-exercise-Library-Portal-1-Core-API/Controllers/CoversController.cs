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
    public class CoversController : Controller
    {
        private readonly LibraryDbContext _context;

        public CoversController(LibraryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/Covers
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_context
                        .Cover
                        .ToList()
                        .Select(c => new CoverData(c.Id,
                            c.Image)));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Covers/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Covers
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Covers/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
