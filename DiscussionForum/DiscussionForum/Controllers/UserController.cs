using DiscussionForum.Data.Interfaces;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> UserQuestions()
        {
            IEnumerable<Question> questions = await _userRepository.GetQuestionsByUserAsync();
            return View(questions);
        }
    }
}
