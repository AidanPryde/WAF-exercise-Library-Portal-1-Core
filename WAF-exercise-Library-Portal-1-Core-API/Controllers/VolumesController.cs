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

        // POST: api/Volumes
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Post([FromBody] VolumeData volumeData)
        {
            try
            {
                if (volumeData.BookData == null)
                {
                    return NoContent();
                }

                var addedVolume = _context.Volume.Add(new Volume
                {
                    Id = volumeData.Id,
                    BookId = volumeData.BookData.Id,
                    IsSordtedOut = volumeData.IsSortedOut
                });

                _context.SaveChanges();

                volumeData.Id = addedVolume.Entity.Id;

                return Ok(volumeData.Id);

            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Volumes/5
        [Authorize(Roles = "admin")]
        [HttpPut]
        public IActionResult Put([FromBody] VolumeData volumeData)
        {
            try
            {
                Volume volume = _context.Volume.FirstOrDefault(b => b.Id == volumeData.Id);

                if (volume == null)
                {
                    return NotFound();
                }

                if (volumeData.BookData == null)
                {
                    return NoContent();
                }

                volume.BookId = volumeData.BookData.Id;
                volume.IsSordtedOut = volumeData.IsSortedOut;

                _context.SaveChanges();

                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Volumes/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(String id)
        {
            try
            {
                Volume volume = _context.Volume.FirstOrDefault(v => v.Id == id);

                if (volume == null)
                {
                    return NotFound();
                }

                _context.Volume.Remove(volume);
                _context.SaveChanges();

                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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
