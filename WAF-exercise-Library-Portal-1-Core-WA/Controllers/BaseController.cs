using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using WAF_exercise_Library_Portal_1_Core_WA.Services;

namespace WAF_exercise_Library_Portal_1_Core_WA.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILibraryService _libraryService;

        public BaseController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            ViewBag.CurrentApplicationUserName = User.Identity.Name;
        }
    }
}