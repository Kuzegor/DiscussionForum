using DiscussionForum.Data;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;
        public QuestionController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Question> questions = _context.Questions.ToList();
            return View(questions);
        }
    }
}
