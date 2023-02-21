using DiscussionForum.Data;
using DiscussionForum.Data.Interfaces;
using DiscussionForum.Data.Repository;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Hosting;
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

        public async Task<IActionResult> QuestionAnswers()
        {
            IEnumerable<Answer> answers = await _answerRepository.GetByQuestionId(int.Parse(TempData["currentQuestionId"].ToString()));
            return PartialView("_QuestionAnswers",answers);
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

        public async Task<IActionResult> Delete(int id)
        {
            Answer answer = await _answerRepository.GetByIdAsync(id);
            if (answer != null)
            {
                return View(answer);
            }
            else
            {
                return View("Error");
            }
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            Answer answer = await _answerRepository.GetByIdAsync(id);
            if (answer != null)
            {
                
                _answerRepository.Delete(answer);
                if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("UserAnswers", "User");
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            Answer answer = await _answerRepository.GetByIdAsync(id);
            return View(answer);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Answer answer)
        {
            if (ModelState.IsValid == false)
            {
                return View(answer);
            }

            _answerRepository.Update(answer);
            return RedirectToAction("Detail", "Question", new { id = answer.QuestionId });
        }
    }
}
