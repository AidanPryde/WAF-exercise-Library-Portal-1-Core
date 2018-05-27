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
    public class LendingsController : Controller
    {
        private readonly LibraryDbContext _context;

        public LendingsController(LibraryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/Lendings
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_context
                        .Lending
                        .ToList()
                        .Select(l => new LendingData(l.Id,
                            l.ApplicationUser.Name,
                            l.StartDate,
                            l.EndDate,
                            l.Active,
                            new VolumeData(l.VolumeId)
                        )));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Lendings/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Lendings
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Lendings/5
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
