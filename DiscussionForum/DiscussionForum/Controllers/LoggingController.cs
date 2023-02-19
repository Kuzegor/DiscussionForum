using DiscussionForum.Data;
using DiscussionForum.Models;
using DiscussionForum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    public class LoggingController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public LoggingController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(loginViewModel);
            }

            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user != null)
            {
                bool passwordIsCorrect = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordIsCorrect)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Question");
                    }
                    else
                    {
                        TempData["Error"] = "Sorry, Something went wrong. Please, check credentials";
                        return View(loginViewModel);
                    }
                }
                TempData["Error"] = "Sorry, wrong credentials.";
                return View(loginViewModel);
            }
            TempData["Error"] = "Sorry, wrong credentials.";
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(registerViewModel);
            }

            var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if (user != null)
            {
                TempData["Error"] = "Sorry, this email is already registered.";
                return View(registerViewModel);
            }
            else
            {
                var newUser = new AppUser()
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email,
                    EmailConfirmed = true
                };
                var signUpResult = await _userManager.CreateAsync(newUser,registerViewModel.Password);
                if (signUpResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                    return RedirectToAction("Login", "Logging");
                }
                else
                {
                    TempData["Error"] = "Sorry, something went wrong. Please, check credentials";
                    return View(registerViewModel);
                }              
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Question");
        }
    }
}
