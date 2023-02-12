using DiscussionForum.Data;
using DiscussionForum.Data.Interfaces;
using DiscussionForum.Data.Repository;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    public class AnswerController : Controller
    {
        private readonly IAnswerRepository _answerRepository;
        public AnswerController(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public IActionResult Create()
        {
            return PartialView("_Create");
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
