using Kyrsach.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kyrsach.Components
{
    public class ListUserViewComponent : ViewComponent
    {
        private readonly SerovaContext _context;
        public ListUserViewComponent(SerovaContext context)
        {
            _context = context;
        }
        public string Invoke(string idUser)
        {

            var tests = _context.Tests
                .Join(_context.Questions, test => test.IdTest, question => question.IdTest, (test, question) => new { Tests = test, Questions = question })
                .Join(_context.Answers, q => q.Questions.IdQuestion, answer => answer.IdQuestion, (q, answer) => new { TestQuestion = q, Answers = answer })
                .Join(_context.AnswersUsers, a => a.Answers.IdAnswersUser, answerUser => answerUser.IdAnswersUser, (a, answerUser) => new { TestQuestionAnswer = a, AnswersUsers = answerUser })
                .Where(x => x.AnswersUsers.IdUser == idUser)
                .Select(x => x.TestQuestionAnswer.TestQuestion.Tests)
                .Distinct()
                .ToList();
            string t = "";
            foreach (var i in tests)
            {
                t += i.NameTest + "|";
            }

            return t;
        }
    }
}
