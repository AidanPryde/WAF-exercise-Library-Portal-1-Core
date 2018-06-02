using System;
using System.Collections.Generic;
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

                if (_context.Book.FirstOrDefault(b => volumeData.BookData.Id == b.Id) == null)
                {
                    return StatusCode(StatusCodes.Status412PreconditionFailed);
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

                if (volumeData.BookData == null
                 || _context.Book.FirstOrDefault(b => volumeData.BookData.Id == b.Id) == null)
                {
                    return StatusCode(StatusCodes.Status412PreconditionFailed);
                }

                List<Lending> lendings = new List<Lending>(_context.Lending.Where(l => l.VolumeId == volume.Id));

                if (lendings != null)
                {
                    foreach (Lending lending in lendings)
                    {
                        if (IsStopingSortingOutVolumeLending(lending))
                        {
                            return StatusCode(StatusCodes.Status412PreconditionFailed);
                        }
                    }
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

        private LendingState GetLendingState(Lending lending)
        {
            DateTime now = DateTime.UtcNow;

            if (now < lending.StartDate)
            {
                if (lending.Active == 0)
                {
                    return LendingState.TOO_SOON_TO_PICK_UP;
                }
                else
                {
                    return LendingState.ERROR;
                }
            }

            if (now < lending.EndDate)
            {
                if (lending.Active == 0)
                {
                    return LendingState.READY_TO_PICK_UP;
                }
                else if (lending.Active == 1)
                {
                    return LendingState.PICKED_UP;
                }
                else
                {
                    return LendingState.RETURNED;
                }
            }

            if (lending.Active == 0)
            {
                return LendingState.NOT_PICKED_UP;
            }
            else if (lending.Active == 1)
            {
                return LendingState.NOT_RETURNED;
            }
            else
            {
                return LendingState.RETURNED;
            }
        }
        private Boolean IsStopingSortingOutVolumeLending(Lending lending)
        {
            LendingState lendingState = GetLendingState(lending);

            if (lendingState == LendingState.ERROR
             || lendingState == LendingState.NOT_RETURNED
             || lendingState == LendingState.PICKED_UP)
            {
                return true;
            }

            return false;
        }
    }
}
