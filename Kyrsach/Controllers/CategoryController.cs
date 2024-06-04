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
        public async Task<IActionResult> Index(string type, string NameCat="")
        {
            try { 
            type = type == "fieldImg" ? "field" : type;
            type = type == "fieldText" ? "field" : type;
            var userId = User.Claims.Where(u => u.Type == "id")?.FirstOrDefault()?.Value;
            var categories = new List<Category>();
            if (!User.IsInRole("admin"))
            {
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
                        Category c = new Category
                        {
                            IdCategory = (int)reader["IdCategory"],
                            NameCategory = (string)reader["NameCategory"],
                            WhoCreatedCategory = (string)(reader["WhoCreatedCategory"]),
                            Picture = reader["Picture"] is byte[]? (byte[])reader["Picture"] : null,
                            Type = type
                        };


                        categories.Add(c);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            else
            {
                categories = _context.Category.Where(c => c.Type.Contains(type)).ToList();
            }
                categories = categories.Where(c => c.NameCategory.ToLower().Contains(NameCat.ToLower())).ToList();
                switch (type)
            {
                case "test":
                    return View(categories);
                case "quez":
                 return View("IndexQuez", categories);
                case "field":
                    return View("IndexField", categories);
            }
            return View(categories);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }


        [Authorize(Roles = "teacher,admin")]

        public IActionResult Create()
        {
            return View();
        }
[Authorize(Roles = "teacher,admin")]
        public IActionResult CreateQuez()
        {
            return View();
        }
[Authorize(Roles = "teacher,admin")]
        public IActionResult CreateField()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "teacher,admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategory,NameCategory,WhoCreatedCategory,Picture, Type")] CategoryViewModel CategoryView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category category = new Category
                    {
                        NameCategory = CategoryView.NameCategory,
                        Type = CategoryView.Type,
                        WhoCreatedCategory = CategoryView.WhoCreatedCategory
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
                            return RedirectToAction(nameof(Index), new { type = "test" });
                        case "quez":
                            return RedirectToAction(nameof(Index), new { type = "quez" });
                        case "fieldImg":
                            return RedirectToAction(nameof(Index), new { type = "field" });
                        case "fieldText":
                            return RedirectToAction(nameof(Index), new { type = "field" });
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
                return RedirectToAction(nameof(Create), CategoryView);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }

[Authorize(Roles = "teacher,admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            try { 
            if (id == null || _context.Category == null)
            {
                    return RedirectToAction("Index", "Errors", new { message = "Невозможно выполнить операцию. Повторите попытку позже." });
                }

                var Category = await _context.Category.FindAsync(id);
            //ViewBag.IdC = id;
            if (Category == null)
            {
                    return RedirectToAction("Index", "Errors", new { message="Невозможно выполнить операцию. Повторите попытку позже." });
                }
            return View(Category);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }
[Authorize(Roles = "teacher,admin")]
        public async Task<IActionResult> EditQuez(int? id)
        {
            try { 
            if (id == null || _context.Category == null)
            {
                return RedirectToAction("Index", "Errors", new { message = "Невозможно выполнить операцию. Повторите попытку позже." });
            }

            var Category = await _context.Category.FindAsync(id);
            //ViewBag.IdC = id;
            if (Category == null)
            {
                return RedirectToAction("Index", "Errors", new { message = "Невозможно выполнить операцию. Повторите попытку позже." });
            }
            return View(Category);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }
[Authorize(Roles = "teacher,admin")]
        public async Task<IActionResult> EditField(int? id)
        {
            try { 
            if (id == null || _context.Category == null)
            {
                return RedirectToAction("Index", "Errors", new { message = "Невозможно выполнить операцию. Повторите попытку позже." });
            }

            var Category = await _context.Category.FindAsync(id);
            //ViewBag.IdC = id;
            if (Category == null)
            {
                return RedirectToAction("Index", "Errors", new { message = "Невозможно выполнить операцию. Повторите попытку позже." });
            }
            return View(Category);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }


        [HttpPost]
        [Authorize(Roles = "teacher,admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategory,NameCategory,WhoCreatedCategory,Picture,Type")] Category сategory, IFormFile? pic)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = await _context.Category.FindAsync(id); 
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
                                return RedirectToAction(nameof(Index), new { type = "field" });
                            case "fieldText":
                                return RedirectToAction(nameof(Index), new { type = "field" });
                        }
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Errors", new { message = ex.Message.Split('.')[0] });

                    }
                }
            } return View(сategory);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "teacher,admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            try { 
            if (id == null || _context.Category == null) return NotFound();
            var Category = await _context.Category
                .FirstOrDefaultAsync(m => m.IdCategory == id);
            if (Category == null) return NotFound(); 
            return View(Category);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "teacher,admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try { 
            if (_context.Category == null)  return Problem("Entity set 'SerovaContext.Сategories'  is null."); 
            var Category = await _context.Category.FindAsync(id);
            var type = Category.Type;
            if (Category != null)  _context.Category.Remove(Category);  
            await _context.SaveChangesAsync();
            switch (Category.Type)
            {
                case "test":
                    return RedirectToAction(nameof(Index), new { type = "test" });
                case "quez":
                    return RedirectToAction(nameof(Index), new { type = "quez" });
                case "fieldImg":
                    return RedirectToAction(nameof(Index), new { type = "field" });
                case "fieldText":
                    return RedirectToAction(nameof(Index), new { type = "field" });
            }
            return View(Category);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }

        private bool CategoryExists(int id)
        {
            return (_context.Category?.Any(e => e.IdCategory == id)).GetValueOrDefault();
        }
    }
}
