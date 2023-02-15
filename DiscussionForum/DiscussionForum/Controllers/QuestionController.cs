using DiscussionForum.Data;
using DiscussionForum.Data.Interfaces;
using DiscussionForum.Data.Repository;
using DiscussionForum.Helpers;
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
            IEnumerable<Question> questions = await _questionRepository.GetAllAsync();
            return View(questions);
        }

        public async Task<IActionResult> UserQuestions()
        {
            //TODO: MAKE UserQuestions ACTION ACTUALLY RETURN QUESTIONS BY USER
            IEnumerable<Question> questions = await _questionRepository.GetAllAsync();
            return View(questions);
        }

        public async Task<IActionResult> Detail(int id)
        {
            TempData["currentQuestionId"] = id;
            Question question = await _questionRepository.GetByIdAsync(id);
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
                if (ImageHelper.ImageIsValid(question.ImageFile))
                {
                    //saving the uploaded image
                    string folder = "img/uploaded/" + Guid.NewGuid().ToString() + question.ImageFile.FileName;
                    string serverPath = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    using (FileStream fileStream = new FileStream(serverPath, FileMode.Create))
                    {
                        await question.ImageFile.CopyToAsync(fileStream);
                    }
                    question.Image = "~/" + folder;
                }
                else
                {
                    ModelState.AddModelError("", "Image is not valid");
                    return View(question);
                }
            }

            if (ModelState.IsValid == false)
            {
                return View(question);
            }
            _questionRepository.Add(question);
            return RedirectToAction("Detail", new {id = question.Id});
        }

        public async Task<IActionResult> Edit(int id)
        {
            //TODO: IMAGE LOADIING TO THE EDIT PAGE
            Question question = await _questionRepository.GetByIdAsync(id);
            TempData["previousImagePath"] = question.Image;
            return View(question);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Question question)
        {
            if (question.ImageFile != null)
            {
                if (ImageHelper.ImageIsValid(question.ImageFile))
                {
                    //saving the new image
                    string folder = "img/uploaded/" + Guid.NewGuid().ToString() + question.ImageFile.FileName;
                    string serverPath = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    using (FileStream fileStream = new FileStream(serverPath, FileMode.Create))
                    {
                        await question.ImageFile.CopyToAsync(fileStream);
                    }
                    question.Image = "~/" + folder;

                    //removing the old image
                    if (TempData["previousImagePath"] != null)
                    {
                        string oldImageFolder = TempData["previousImagePath"].ToString().Remove(0, 2);
                        System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, oldImageFolder));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Image is not valid");
                    return View(question);
                }
            }
            else
            {
                question.Image = TempData["previousImagePath"]?.ToString();
            }

            if (ModelState.IsValid == false)
            {
                return View(question);
            }
            _questionRepository.Update(question);
            return RedirectToAction("Detail", new { id = question.Id });
        }
    }
}
