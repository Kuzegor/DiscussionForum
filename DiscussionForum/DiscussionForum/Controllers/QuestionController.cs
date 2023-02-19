using DiscussionForum.Data;
using DiscussionForum.Data.Enums;
using DiscussionForum.Data.Interfaces;
using DiscussionForum.Data.Repository;
using DiscussionForum.Helpers;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace DiscussionForum.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public QuestionController(IQuestionRepository questionRepository, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor contextAccessor)
        {
            _questionRepository = questionRepository;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Question> questions = await _questionRepository.GetAllAsync();
            return View(questions);
        }
        public async Task<IActionResult> Sort(Topic topic)
        {
            IEnumerable<Question> questions = await _questionRepository.GetAllAsync();
            List<Question> filtered = questions.Where(x => x.Topic == topic).ToList();
            return View("Index",filtered);
        }

        public async Task<IActionResult> Detail(int id)
        {
            TempData["currentQuestionId"] = id;
            Question question = await _questionRepository.GetByIdAsync(id);
            if (question != null)
            {
                return View(question);
            }
            else
            {
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Question question = new Question { AppUserId = currentUserId };
            return View(question);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Question question)
        {
            if (ModelState.IsValid == false)
            {
                return View(question);
            }

            //saving the uploaded image
            if (question.ImageFile != null)
            {
                if (ImageHelper.ImageIsValid(question.ImageFile))
                {
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

            _questionRepository.Add(question);
            return RedirectToAction("Detail", new {id = question.Id});
        }

        public async Task<IActionResult> Edit(int id)
        {
            Question question = await _questionRepository.GetByIdAsync(id);
            TempData["previousImagePath"] = question.Image;
            return View(question);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Question question)
        {
            if (ModelState.IsValid == false)
            {
                return View(question);
            }

            //saving the uploaded image
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

            _questionRepository.Update(question);
            return RedirectToAction("Detail", new { id = question.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            Question question = await _questionRepository.GetByIdAsync(id);
            if (question != null)
            {
                return View(question);
            }
            else
            {
                return View("Error");
            }
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            Question question = await _questionRepository.GetByIdAsync(id);
            if (question != null)
            {
                if (question.Image != null)
                {
                    try
                    {
                        string path = Path.Combine(_webHostEnvironment.WebRootPath, question.Image.ToString().Remove(0, 2));
                        System.IO.File.Delete(path);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        _questionRepository.Delete(question);
                        if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
                        {
                            return RedirectToAction("Index");
                        }
                        return RedirectToAction("UserQuestions", "User");
                    }
                }
                _questionRepository.Delete(question);
                if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("UserQuestions", "User");
            }
            else
            {
                return View("Error");
            }
        }
    }
}
