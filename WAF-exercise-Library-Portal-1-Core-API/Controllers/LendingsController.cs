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

        // PUT: api/Lendings/
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

                if (lendingData.StartDate == DateTime.MinValue)
                {
                    lendingData.StartDate = lending.StartDate;
                }

                if (lendingData.EndDate == DateTime.MinValue)
                {
                    lendingData.EndDate = lending.EndDate;
                }

                if (lendingData.StartDate > lendingData.EndDate)
                {
                    return StatusCode(StatusCodes.Status412PreconditionFailed);
                }

                lending.StartDate = lendingData.StartDate;
                lending.EndDate = lendingData.EndDate;

                if (lendingData.Active == 1 && lending.Active == 0 && IsLendableLending(lending))
                {
                    lending.Active = lendingData.Active;
                }
                else if (lendingData.Active == 2 && lending.Active != 2)
                {
                    lending.Active = 1;
                    if (IsReturnableLending(lending))
                    {
                        lending.Active = lendingData.Active;
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status412PreconditionFailed);
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status412PreconditionFailed);
                }

                _context.SaveChanges();

                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Lendings/5
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
        private Boolean IsReturnableLending(Lending lending)
        {
            LendingState lendingState = GetLendingState(lending);

            if (lendingState == LendingState.NOT_RETURNED
             || lendingState == LendingState.PICKED_UP)
            {
                return true;
            }

            return false;
        }
        private Boolean IsLendableLending(Lending lending)
        {
            LendingState lendingState = GetLendingState(lending);

            if (lendingState == LendingState.READY_TO_PICK_UP)
            {
                return true;
            }

            if (lendingState == LendingState.TOO_SOON_TO_PICK_UP)
            {
                
                foreach (Lending otherLending in _context.Lending.Where(l => l.VolumeId == lending.VolumeId))
                {
                    LendingState otherLendingState = GetLendingState(otherLending);

                    if (otherLendingState == LendingState.NOT_RETURNED
                     || otherLendingState == LendingState.PICKED_UP
                     || otherLendingState == LendingState.READY_TO_PICK_UP)
                    {
                        return false;
                    }

                    if (otherLendingState == LendingState.TOO_SOON_TO_PICK_UP
                     && otherLending != lending)
                    {
                        if (otherLending.StartDate < lending.StartDate)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            return false;
        }
    }
}
