using DocumentFormat.OpenXml.Office2010.Excel;
using Kyrsach.Models;
using Kyrsach.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kyrsach.Controllers
{

    //[ApiController]
    //[Route("api/Question")]
    public class QuestionsController : Controller
    {
        private readonly SerovaContext _context;

        public QuestionsController(SerovaContext context)
        {
            _context = context;
        }


        // GET: Question
        public async Task<IActionResult> Index(int idQ, int idT, string? answer, string? message)
        {
            ViewBag.Message = message;
            if (idQ == 0 && _context.Questions.Where(t => t.IdTest == idT).FirstOrDefault() != null)
            {
                ViewBag.idT = idT;
                var userId = User.Claims.Where(u => u.Type == "id")?.FirstOrDefault()?.Value;
                AnswersUser answersUser = new AnswersUser{
                   IdUser= userId, 
                   CountCurrent= 0, 
                   Time= DateTime.Now };
                _context.Add(answersUser);
                await _context.SaveChangesAsync();
                var question = await _context.Questions.FirstOrDefaultAsync(t => t.IdTest == idT);
                ViewBag.Button = "Далее";
                return View(question);
            }else
            {
                ViewBag.idT = idT;
                var question = await  _context.Questions.FirstOrDefaultAsync(t => t.IdTest == idT && t.IdQuestion == idQ);
                ViewBag.Button = "Далее";
                return View(question);
            }
        }
        public async Task<IActionResult> Dalee(int idQ, int idT, string answer)
        {

            try
            {
                if (answer != null)
                {
                    var nextQuestion = GetNextQuestion(idQ, idT);
                    var lastQuestion = GetLastQuestion(idQ, idT);

                    ViewBag.idT = idT;
                    ViewBag.Button = nextQuestion != null ? "Далее" : "Готово";

                    string isEqual = CalculateIsEqual(idQ, answer);

                    var lastAnswerId = _context.AnswersUsers.OrderByDescending(t => t.IdAnswersUser).FirstOrDefault()?.IdAnswersUser ?? 0;
                    var answ = new Answer(lastAnswerId, idQ, answer, isEqual);
                    await _context.AddAsync(answ);
                    await _context.SaveChangesAsync();

                    if (lastQuestion == null)
                    {
                        int countResult = await CalculateAndUpdateCurrentScore(idT);
                        return RedirectToAction(nameof(End), new { count = countResult });
                    }

                    var serovaContext = _context.Questions.Where(t => t.IdTest == idT && t.IdQuestion == (nextQuestion == null ? lastQuestion.IdQuestion : nextQuestion.IdQuestion));
                    return View(nameof(Index), await serovaContext.ToListAsync());
                }
                else
                {
                    return RedirectToAction(nameof(Index), new { idQ, idT, message = "Пожалуйста выберите вариант ответа" });
                }
            }
            catch (Exception ex)
            {
                AnswersUser answ = _context.AnswersUsers.OrderByDescending(t => t.IdAnswersUser).FirstOrDefault();
                _context.AnswersUsers.Remove(answ);
                await _context.SaveChangesAsync();
                var Message = ex.InnerException?.Message?.ToString()?.Split('.')[0];
                return RedirectToAction("Index", "Errors", new { Message });
            }
        }

        private Question GetNextQuestion(int idQ, int idT)
        {
            return _context.Questions.OrderBy(t => t.IdQuestion).FirstOrDefault(t => t.IdTest == idT && t.IdQuestion > idQ);
        }

        private Question GetLastQuestion(int idQ, int idT)
        {
            return _context.Questions.OrderBy(t => t.IdQuestion).LastOrDefault(t => t.IdTest == idT && t.IdQuestion > idQ);
        }

        private string CalculateIsEqual(int idQ, string answer)
        {
            if (string.IsNullOrEmpty(answer))
            {
                return "false";
            }

            var correctAnswer = _context.Questions.FirstOrDefault(t => t.IdQuestion == idQ)?.CorrectAnswer;
            if (correctAnswer == null)
            {
                return "false";
            }

            var q = correctAnswer.Split('|');
            var a = answer.Split('|');
            return q.OrderBy(a => a).SequenceEqual(a.OrderBy(a => a), StringComparer.OrdinalIgnoreCase).ToString();
        }

        private async Task<int> CalculateAndUpdateCurrentScore(int idT)
        {
            var idAnUse = _context.AnswersUsers.OrderByDescending(t => t.IdAnswersUser).FirstOrDefault().IdAnswersUser;
            double all = _context.Answers.Count(t => t.IdAnswersUser == idAnUse);
            double curr = _context.Answers.Count(t => t.IdAnswersUser == idAnUse && t.Loyal == "True");
            var answerU = _context.AnswersUsers.FirstOrDefault(t => t.IdAnswersUser == idAnUse);
            answerU.CountCurrent = (int)curr;
            _context.Entry(answerU).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return (int)(curr / all * 100);
        }

        public async Task<IActionResult> End(int count)
        {
            ViewBag.Count = count;
            return View();
        }



[Authorize(Roles = "teacher,admin")]
        [HttpGet]
        public IActionResult Create(int idT)
        {
            ViewBag.idT = idT;
            ViewData["IdTest"] = new SelectList(_context.Tests, "IdTest", "IdTest");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "teacher,admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionViewModel questionViewModel, int idT, string answer)
        {
            try
            {
                Question question = new Question
                {
                    TextQuetion = questionViewModel.TextQuetion,
                    Answers = questionViewModel.Answers,
                    Option1 = questionViewModel.Option1,
                    Option2 = questionViewModel.Option2,
                    Option3 = questionViewModel.Option3,
                    Option4 = questionViewModel.Option4,
                    Option5 = questionViewModel.Option5,
                    IdTest = questionViewModel.IdTest,

                    CorrectAnswer = answer == null ? questionViewModel.CorrectAnswer : answer
                };
                if (questionViewModel.PictureTest != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(questionViewModel.PictureTest.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)questionViewModel.PictureTest.Length);
                    }
                    // установка массива байтов
                    question.PictureTest = imageData;
                }
                if (question.CorrectAnswer != null)
                {
                    question.IdTest = idT;
                    _context.Add(question);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index), new { idT = question.IdTest });
                }
                else
                {
                    question.IdTest = idT;
                    ViewBag.idT = idT;
                    return View(question);
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
            return View(questionViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> End1()
        {
            return View();
        }
      


        //GET: Questions/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Questions == null)
        //    {
        //        return NotFound();
        //    }

        //    var question = await _context.Questions
        //        .Include(q => q.IdTestNavigation)
        //        .FirstOrDefaultAsync(m => m.IdQuestion == id);
        //    if (question == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(question);
        //}

        ////POST: Questions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id, int idT)
        {
            try { 
            if (_context.Questions == null)
            {
                return Problem("Entity set 'SerovaContext.Questions'  is null.");
            }
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { idT = question.IdTest });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }

        private bool QuestionExists(int id)
        {
            return (_context.Questions?.Any(e => e.IdQuestion == id)).GetValueOrDefault();
        }
    }
}
