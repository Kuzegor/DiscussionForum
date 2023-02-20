using DiscussionForum.Data;
using DiscussionForum.Data.Interfaces;
using DiscussionForum.Data.Repository;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DiscussionForum.Controllers
{
    public class AnswerController : Controller
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AnswerController(IAnswerRepository answerRepository, IHttpContextAccessor httpContextAccessor)
        {
            _answerRepository = answerRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Create()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Answer answer = new Answer { AppUserId = currentUserId };
            return PartialView("_Create",answer);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Answer answer)
        {
            if (ModelState.IsValid == false)
            {
                return RedirectToAction("Detail","Question", new { id = answer.QuestionId });
            }
            _answerRepository.Add(answer);
            return RedirectToAction("Detail", "Question", new { id = answer.QuestionId });
        }
    }
}
