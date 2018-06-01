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

        // POST: api/Covers
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Post([FromBody] CoverData coverData)
        {
            try
            {
                var addedCover = _context.Cover.Add(new Cover
                {
                    Image = coverData.Image
                });

                _context.SaveChanges();

                coverData.Id = addedCover.Entity.Id;

                return Ok(coverData.Id);

            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(Int32 id)
        {
            try
            {
                Cover cover = _context.Cover.FirstOrDefault(c => c.Id == id);

                if (cover == null)
                {
                    return NotFound();
                }

                _context.Cover.Remove(cover);
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
