using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Kyrsach.Models;
using Kyrsach.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Kyrsach.Controllers
{
    public class CategoryController : Controller
    {
        private readonly SerovaContext _context;
       string _connectionString = "Server=(LocalDB)\\MSSQLLocalDB;Database=Serova4;Trusted_Connection=True;MultipleActiveResultSets=true";

        public CategoryController(SerovaContext context)
        {
            _context = context;
        }

        // GET: Category
        [Authorize]
        public async Task<IActionResult> Index(string type)
        {

            var userId = User.Claims.Where(u => u.Type == "id")?.FirstOrDefault()?.Value;
            var categories = new List<Category>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetCategoriesByUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@type", type);
                connection.Open();
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var IdCategory = (int)reader["IdCategory"];
                    var NameCategory = (string)reader["NameCategory"];
                    var WhoCreatedCategory = (reader["WhoCreatedCategory"].ToString());
                    byte[] Picture = (byte[])reader["Picture"];
                    Category c = new Category{ 
                        IdCategory = IdCategory, 
                        NameCategory= NameCategory,
                        WhoCreatedCategory= (string)WhoCreatedCategory, 
                        Picture= Picture,
                        Type = type
                    };
                    categories.Add(c);
                }
                reader.Close();
                connection.Close();
            }

            switch (type)
            {
                case "test":
                    return View(categories.ToList());
                case "quez":
                 return View("IndexQuez", categories.ToList());
                case "field":
                    return View("IndexField", categories.ToList());
            }
            return View(categories.ToList());
        }

       
        [Authorize(Roles="teacher")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "teacher")]
        public IActionResult CreateQuez()
        {
            return View();
        }
        [Authorize(Roles = "teacher")]
        public IActionResult CreateField()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategory,NameCategory,WhoCreatedCategory,Picture, Type")] CategoryViewModel CategoryView)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category
                {
                    NameCategory = CategoryView.NameCategory,
                    Type = CategoryView.Type,
                    WhoCreatedCategory=CategoryView.WhoCreatedCategory
                };
                if (CategoryView.Picture != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(CategoryView.Picture.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)CategoryView.Picture.Length);
                    }
                    // установка массива байтов
                    category.Picture = imageData;
                }
                _context.Add(category);
                await _context.SaveChangesAsync();
                switch (category.Type)
                {
                    case "test":
                        return RedirectToAction(nameof(Index), new {type = "test" });
                    case "quez":
                        return RedirectToAction(nameof(Index), new {type = "quez" });
                    case "fieldImg":
                        return RedirectToAction(nameof(Index), new { type = "fieldImg" });
                    case "fieldText":
                        return RedirectToAction(nameof(Index), new { type = "fieldText" });
                }

            }
			switch (CategoryView.Type)
			{
				case "test":
					return RedirectToAction(nameof(Create), CategoryView);
				case "quez":
					return RedirectToAction(nameof(CreateQuez), CategoryView);
				case "fieldImg":
					return RedirectToAction(nameof(CreateField), CategoryView);
				case "fieldText":
					return RedirectToAction(nameof(CreateField), CategoryView);
			}
            return NotFound();
		}

        [Authorize(Roles = "teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var Category = await _context.Category.FindAsync(id);
            //ViewBag.IdC = id;
            if (Category == null)
            {
                return NotFound();
            }
            return View(Category);
        }
        [Authorize(Roles = "teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuez(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var Category = await _context.Category.FindAsync(id);
            //ViewBag.IdC = id;
            if (Category == null)
            {
                return NotFound();
            }
            return View(Category);
        }
        [Authorize(Roles = "teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditField(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var Category = await _context.Category.FindAsync(id);
            //ViewBag.IdC = id;
            if (Category == null)
            {
                return NotFound();
            }
            return View(Category);
        }


        [HttpPost]
        [Authorize(Roles = "teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategory,NameCategory,WhoCreatedCategory,Picture,Type")] Category сategory, IFormFile pic)
        {


            var existingCategory = await _context.Category.FindAsync(id); // получаем объект из контекста

            if (existingCategory != null)
            {
                existingCategory.NameCategory = сategory.NameCategory;
                existingCategory.WhoCreatedCategory = сategory.WhoCreatedCategory;

                if (pic != null)
                {
                    byte[] imageData = null;

                    using (var binaryReader = new BinaryReader(pic.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)pic.Length);
                    }

                    existingCategory.Picture = imageData;
                }

                try
                {
                    await _context.SaveChangesAsync();

                    switch (existingCategory.Type)
                    {
                        case "test":
                            return RedirectToAction(nameof(Index), new { type = "test" });
                        case "quez":
                            return RedirectToAction(nameof(Index), new { type = "quez" });
                        case "fieldImg":
                            return RedirectToAction(nameof(Index), new { type = "fieldImg" });
                        case "fieldText":
                            return RedirectToAction(nameof(Index), new { type = "fieldText" });
                    }
                }
                catch (Exception ex)
                {
                        return RedirectToAction("Index","Errors",new {message = ex.Message.Split('.')[0] });
                   
                }
            }

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var Category = await _context.Category
                .FirstOrDefaultAsync(m => m.IdCategory == id);
            if (Category == null)
            {
                return NotFound();
            }

            return View(Category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Category == null)
            {
                return Problem("Entity set 'SerovaContext.Сategories'  is null.");
            }
            var Category = await _context.Category.FindAsync(id);
            var type = Category.Type;
            if (Category != null)
            {
                _context.Category.Remove(Category);
            }

            await _context.SaveChangesAsync();
            switch (Category.Type)
            {
                case "test":
                    return RedirectToAction(nameof(Index), new { type = "test" });
                case "quez":
                    return RedirectToAction(nameof(Index), new { type = "quez" });
                case "fieldImg":
                    return RedirectToAction(nameof(Index), new { type = "fieldImg" });
                case "fieldText":
                    return RedirectToAction(nameof(Index), new { type = "fieldText" });
            }
            return View(Category);
        }

        private bool CategoryExists(int id)
        {
            return (_context.Category?.Any(e => e.IdCategory == id)).GetValueOrDefault();
        }
    }
}
