using System;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using WAF_exercise_Library_Portal_1_Core_Db;
using WAF_exercise_Library_Portal_1_Core_Db.Models;
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
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_context
                        .Lending
                        .Include(l => l.ApplicationUser)
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
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Lendings/5
        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody] LendingData lendingData)
        {
            try
            {
                Lending lending = _context.Lending.FirstOrDefault(l => l.Id == lendingData.Id);

                if (lending == null)
                {
                    return NotFound();
                }

                lending.Active = lendingData.Active;
                lending.StartDate = lendingData.StartDate;
                lending.EndDate = lendingData.EndDate;
                lending.VolumeId = lendingData.VolumeData?.Id;

                _context.SaveChanges();

                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/ApiWithActions/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(Int32 id)
        {
            try
            {
                Lending lending = _context.Lending.FirstOrDefault(l => l.Id == id);

                if (lending == null)
                {
                    return NotFound();
                }

                _context.Lending.Remove(lending);
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
