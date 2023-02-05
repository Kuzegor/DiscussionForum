using DiscussionForum.Data;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    public class AnswerController : Controller
    {
        private readonly AppDbContext _context;
        public AnswerController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Answer> answers = _context.Answers.ToList();
            return View(answers);
        }
    }
}
