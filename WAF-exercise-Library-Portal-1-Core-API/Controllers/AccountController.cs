using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WAF_exercise_Library_Portal_1_Core_Db.Models;
using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;

namespace WAF_exercise_Library_Portal_1_Core_API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // api/Account/Login
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginData user)
        {
            try
            {
                if (_signInManager.IsSignedIn(User))
                {
                    await _signInManager.SignOutAsync();
                }

                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, isPersistent: false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        return Ok();
                    }

                    ModelState.AddModelError("", "Login failed!");
                    return Unauthorized();
                }
                
                return Unauthorized();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // api/Account/Logout
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
