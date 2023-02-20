using DiscussionForum.Data.Interfaces;
using DiscussionForum.Data.Repository;
using DiscussionForum.Helpers;
using DiscussionForum.Models;
using DiscussionForum.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace DiscussionForum.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<AppUser> users = await _userRepository.GetAllAsync();
            return View(users);
        }

        public async Task<IActionResult> UserQuestions()
        {
            IEnumerable<Question> questions = await _userRepository.GetQuestionsByUserAsync();
            return View(questions);
        }
        
        public async Task<IActionResult> UserAnswers()
        {
            IEnumerable<Answer> answers = await _userRepository.GetAnswersByUserAsync();
            return View(answers);
        }

        public async Task<IActionResult> Detail(string id)
        {
            AppUser user = await _userRepository.GetByIdAsync(id);
            return View(user);
        }

        public async Task<IActionResult> EditProfile()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser user = await _userRepository.GetByIdAsync(currentUserId);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(AppUser user)
        {
            if (ModelState.IsValid == false)
            {
                return View(user);
            }

            AppUser userNoTracking = await _userRepository.GetByIdNoTrackingAsync(user.Id);

            //saving the uploaded image
            //TODO: Replace this with ImageHelper methods
            if (user.ImageFile != null)
            {
                if (ImageHelper.ImageIsValid(user.ImageFile))
                {
                    //saving the new image
                    string folder = "img/uploaded/" + Guid.NewGuid().ToString() + user.ImageFile.FileName;
                    string serverPath = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    using (FileStream fileStream = new FileStream(serverPath, FileMode.Create))
                    {
                        await user.ImageFile.CopyToAsync(fileStream);
                    }                    

                    //removing the old image
                    if (user.Image != null)
                    {
                        try
                        {
                            string oldImageFolder = user.Image.ToString().Remove(0, 2);
                            System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, oldImageFolder));
                        }
                        catch (DirectoryNotFoundException)
                        {
                            //do nothing
                        }
                    }

                    user.Image = "~/" + folder;
                }
                else
                {
                    ModelState.AddModelError("", "Image is not valid");
                    return View(user);
                }
            }

            userNoTracking.Id = user.Id;
            userNoTracking.UserName = user.UserName;
            userNoTracking.Email = user.Email;
            userNoTracking.Image = user.Image;

            _userRepository.Update(userNoTracking);
            return RedirectToAction("Detail", new { id = user.Id });
        }
    }
}
