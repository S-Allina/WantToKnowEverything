using Kyrsach.Models;
using Kyrsach.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kyrsach.Controllers
{

    public class AnswersUsersController : Controller
    {
        private readonly SerovaContext _context;
        List<UserModel> listUser = new();

        public AnswersUsersController(SerovaContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: AnswersUsers
        public async Task<IActionResult> Index()
        {
            var groups = await _context.Groups.ToListAsync();

            // Создаем список объектов SelectListItem для заполнения выпадающего списка
            var selectListItems = groups.Select(c => new SelectListItem
            {
                Value = c.IdGroup.ToString(), // Здесь должно быть строковое значение идентификатора категории
                Text = c.NameGroup // Здесь должно быть название категории
            }).ToList();
            ViewBag.Groups = selectListItems;
            var categories = await _context.Category.Where(c=>c.Type=="test").ToListAsync();

            // Создаем список объектов SelectListItem для заполнения выпадающего списка
            var selectListItems2 = categories.Select(c => new SelectListItem
            {
                Value = c.IdCategory.ToString(), // Здесь должно быть строковое значение идентификатора категории
                Text = c.NameCategory // Здесь должно быть название категории
            }).ToList();
            ViewBag.Categories = selectListItems2;

            var listUser = _context.Users.ToList();
            return View(listUser);
        }
        [Authorize]
        // GET: AnswersUsers
        public async Task<IActionResult> IndexForUser()
        {
            var userId = User.Claims.Where(u => u.Type == "id")?.FirstOrDefault()?.Value;
            ViewBag.UserId = userId;
            var listUser = _context.Tests.ToList();
            return View(listUser);
        }
      

    }
}
