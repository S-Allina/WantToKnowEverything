using ClosedXML.Excel;
using Kyrsach.Models;
using Kyrsach.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kyrsach.Controllers
{
    public class DiagramsController : Controller
    {
        private readonly string _connectionString;
        private readonly SerovaContext _context;

        public DiagramsController(SerovaContext context)
        {
            _connectionString = "Server=(LocalDB)\\MSSQLLocalDB;Database=Serova4;Trusted_Connection=True;MultipleActiveResultSets=true";

            _context = context;
        }
        public class GetCategoryModel
        {
            public int IdCategory { get; set; }
        }
        [HttpPost]
        public async Task<FileResult> ExportPeopleInExcel(int idCategory, int idGroup)
        {

                var result = new List<ExcelResultViewModel>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand("GetExcel", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CategoryId", idCategory);
                command.Parameters.AddWithValue("@GroupId", idGroup);
                connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var userName = (string)reader["UserName"];
                        var testName = (string)reader["NameTest"];
                        var averageScore = int.Parse(reader["AverageScore"].ToString());
                        result.Add(new ExcelResultViewModel(testName, userName, averageScore));
                    }
                    reader.Close();
                    connection.Close();
                }
                var fileName = $"Result.xlsx";
                return GenerateExcel(fileName, result);
           
        }

        [HttpPost]
        public async Task<FileResult> ExportTestByDateInExcel(string DateStart, string DateEnd,string idGroup)
        {

                var result = new List<ExcelResultViewModel>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand("GetExcelByDate", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DateStart", DateStart);
                    command.Parameters.AddWithValue("@DateEnd", DateEnd);
                command.Parameters.AddWithValue("@idGroup", idGroup);
                connection.Open();
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var userName = (string)reader["UserName"];
                        var testName = (string)reader["NameTest"];
                        var averageScore = int.Parse(reader["AverageScore"].ToString());
                        result.Add(new ExcelResultViewModel(testName, userName, averageScore));
                    }
                    reader.Close();
                    connection.Close();
                }
                var fileName = "ResultByDate.xlsx";
                return GenerateExcel(fileName, result);
            
        }



        private FileResult GenerateExcel(string fileName, List<ExcelResultViewModel> result)
        {
            DataTable dataTable = new DataTable("Result");
            dataTable.Columns.AddRange(new DataColumn[]
            { new DataColumn("Пользователь")
            });
            var oldTest = new List<string>();
            for (int i = 0; i < result.Count(); i++)
            {if (oldTest.Where(t => t == result[i].NameTest).ToList().Count != 0) continue;
                dataTable.Columns.Add(new DataColumn(result[i].NameTest));
                oldTest.Add(result[i].NameTest);
            }
            var oldResult = new List<string>();
            for (int i = 0; i < result.Count(); i++)
            { if (oldResult.Where(t => t.ToString() == result[i].UserName).ToList().Count != 0) continue;
                var item = new List<int>(); // Создайте новый список item в каждой итерации
                foreach (var av in result.Where(t => t.UserName == result[i].UserName))
                {  item.Add(av.AverageScore);
                }
                oldResult.Add(result[i].UserName);
                var dataRow = dataTable.NewRow();
                dataRow["Пользователь"] = result[i].UserName;
                int k = 0;
                foreach (var io in item)
                { dataRow[result[k].NameTest] = io;
                    k++; }
                dataTable.Rows.Add(dataRow);}
            using (XLWorkbook wb = new XLWorkbook())
            {  wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                { wb.SaveAs(stream);
 return File(stream.ToArray(),  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }


        [HttpGet]
        public IActionResult GetTests(string idUser)
        {
            try
            {
                var options = _context.Category
                    .Join(_context.Tests, category => category.IdCategory, test => test.IdCategory, (category, test) => new { Category = category, Test = test })
                    .Join(_context.Questions, test => test.Test.IdTest, question => question.IdTest, (test, question) => new { Tests = test, Questions = question })
                    .Join(_context.Answers, q => q.Questions.IdQuestion, answer => answer.IdQuestion, (q, answer) => new { TestQuestion = q, Answers = answer })
                    .Join(_context.AnswersUsers, a => a.Answers.IdAnswersUser, answerUser => answerUser.IdAnswersUser, (a, answerUser) => new { TestQuestionAnswer = a, AnswersUsers = answerUser })
                    .Where(x => x.AnswersUsers.IdUser == idUser)
                    .Select(x => new { x.TestQuestionAnswer.TestQuestion.Tests.Category.NameCategory, x.TestQuestionAnswer.TestQuestion.Tests.Category.IdCategory })
                    .Distinct()
                    .ToList();
                return Json(options);
            } 
            catch(Exception ex) 
            {
                return RedirectToAction("Index", "Errors", new { ex.Message});
            }
        }

        [HttpGet]
        public IActionResult GetAllTests()
        {
            try { 
            var options = _context.Category
                .Join(_context.Tests, category => category.IdCategory, test => test.IdCategory, (category, test) => new { Category = category, Test = test })
                .Join(_context.Questions, test => test.Test.IdTest, question => question.IdTest, (test, question) => new { Tests = test, Questions = question })
                .Join(_context.Answers, q => q.Questions.IdQuestion, answer => answer.IdQuestion, (q, answer) => new { TestQuestion = q, Answers = answer })
                .Join(_context.AnswersUsers, a => a.Answers.IdAnswersUser, answerUser => answerUser.IdAnswersUser, (a, answerUser) => new { TestQuestionAnswer = a, AnswersUsers = answerUser })
                .Select(x => new { x.TestQuestionAnswer.TestQuestion.Tests.Category.NameCategory, x.TestQuestionAnswer.TestQuestion.Tests.Category.IdCategory })
                .Distinct()
                .ToList();
            return Json(options);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }

        public class GetDataModel
        {
            public string IdUser { get; set; }
            public int IdCategory { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
        [HttpPost]
        public IActionResult GetResultTest([FromBody] GetDataModel data)
        {
            try { 
            ViewBag.idCategory = data.IdCategory;
            var result2 = new { NameTest = new List<string>(), AverageScore = new List<int>() };
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetTestStatistics", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryId", data.IdCategory);
                command.Parameters.AddWithValue("@UserId", data.IdUser);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var testName = (string)reader["NameTest"];
                    var averageScore = int.Parse(reader["AverageScore"].ToString());
                    result2.NameTest.Add(testName);
                    result2.AverageScore.Add(averageScore);
                }
                reader.Close();
                connection.Close();
            }
            return Json(result2);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }
        [HttpPost]
        public IActionResult GetPerformanceTest([FromBody] GetDataModel data)
        {
            try { 
            var result2 = new List<int>();
            ViewBag.idCategory= data.IdCategory;
                using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetPerformanceStatistics", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryId", data.IdCategory);
                command.Parameters.AddWithValue("@UserId", data.IdUser);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {

                    result2.Add(int.Parse(reader["Count_0_3"].ToString()));
                    result2.Add(int.Parse(reader["Count_3_10"].ToString()));
                    result2.Add(int.Parse(reader["Count_7_10"].ToString()));
                }
                reader.Close();
                connection.Close();
            }
                return Json(result2);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }


        [HttpGet]
        public IActionResult GetResultCategory(string idUser)
        {
            try { 
            var result2 = new { NameCategory = new List<string>(), AverageScore = new List<double>() };

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetCategoryStatistics", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@UserId", idUser);

                connection.Open();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var testName = (string)reader["NameCategory"];
                    var averageScore = double.Parse(reader["AverageScore"].ToString());
                    //};
                    result2.NameCategory.Add(testName);
                    result2.AverageScore.Add(averageScore);

                }

                reader.Close();
                connection.Close();
            }
            return Json(result2);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }
    }
}
