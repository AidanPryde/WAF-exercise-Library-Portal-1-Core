using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WAF_exercise_Library_Portal_1_Core_Db.Models;
using WAF_exercise_Library_Portal_1_Core_WA.Services;
using WAF_exercise_Library_Portal_1_Core_WA.Models;

namespace WAF_exercise_Library_Portal_1_Core_WA.Controllers
{
    [Authorize]
    public class LendingController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public LendingController(ILibraryService libraryService, UserManager<ApplicationUser> userManager)
            : base(libraryService)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(String id, LendingViewModel lendingViewModel)
        {
            if (String.IsNullOrEmpty(id))
                return RedirectToAction("Index", "Home");

            if (User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Index", "Home");
            }

            Volume volume = _libraryService.GetVolumeByVolumeId(id);

            if (volume == null)
                return RedirectToAction("Index", "Home");

            lendingViewModel.StartDate = DateTime.UtcNow.AddDays(1);
            lendingViewModel.EndDate = lendingViewModel.StartDate.AddDays(7);
            lendingViewModel.VolumeId = id;

            return View("Index", lendingViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(LendingViewModel lendingViewModel)
        {
            if (lendingViewModel == null)
            {
                ViewBag.Result = "Something went wrong.";
                return View("Index", lendingViewModel);
            }

            if (User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Index", "Home");
            }

            ApplicationUser applicationUser = await _userManager.FindByNameAsync(User.Identity.Name);

            UpdateResult updateResult = await _libraryService.SaveLending(applicationUser.Id, lendingViewModel);
            if (updateResult == UpdateResult.Success)
            {
                ViewBag.Result = "Successfully saved the lending.";
            }
            else
            {
                if (updateResult == UpdateResult.ConcurrencyError)
                {
                    ViewBag.Result = "Wrong intervals with the date times.";
                }
                else
                {
                    ViewBag.Result = "Something went wrong.";
                }
            }
            ViewBag.ReturnBookId = _libraryService.GetBookIdByVolumeId(lendingViewModel.VolumeId);

            return View("Index", lendingViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Int32? id)
        {
            Int32 lendingId = id ?? -1;
            if (lendingId == -1)
                return RedirectToPage(nameof(HomeController));

            if (User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnBookId = _libraryService.GetBookIdByLendingId(lendingId);

            ApplicationUser applicationUser = await _userManager.FindByNameAsync(User.Identity.Name);

            UpdateResult updateResult = await _libraryService.RemoveLending(lendingId, applicationUser.Id);
            if (updateResult == UpdateResult.Success)
            {
                ViewBag.Result = "Successfully removed the lending.";
            }
            else
            {
                ViewBag.Result = "Something went wrong.";
            }

            return View("Result");
        }
    }
}