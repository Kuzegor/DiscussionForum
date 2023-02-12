using DiscussionForum.Data;
using DiscussionForum.Data.Interfaces;
using DiscussionForum.Data.Repository;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DiscussionForum.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public QuestionController(IQuestionRepository questionRepository, IWebHostEnvironment webHostEnvironment)
        {
            _questionRepository = questionRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Question> questions = await _questionRepository.GetAll();
            return View(questions);
        }
        public async Task<IActionResult> Detail(int id)
        {
            TempData["currentQuestionId"] = id;
            Question question = await _questionRepository.GetById(id);
            return View(question);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Question question)
        {
            if (question.ImageFile != null)
            {
                string fileNameLower = question.ImageFile.FileName.ToLower();
                string[] extensions = { "jpg", "png", "jfif", "jpeg", "gif"};
                bool fileIsValid = false;
                for (int i = 0; i < extensions.Length; i++)
                {
                    if (fileNameLower.EndsWith(extensions[i]))
                    {
                        fileIsValid = true;
                        break;
                    }
                }
                if (fileIsValid)
                {
                    string folder = "img/uploaded/" + Guid.NewGuid().ToString() + question.ImageFile.FileName;
                    string serverPath = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    await question.ImageFile.CopyToAsync(new FileStream(serverPath, FileMode.Create));
                    question.Image = "~/" + folder;
                }              
            }
            if (ModelState.IsValid == false)
            {
                return View(question);
            }
            _questionRepository.Add(question);
            return RedirectToAction("Detail", new {id = question.Id});
        }
    }
}
