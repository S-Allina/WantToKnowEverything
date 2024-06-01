
using Kyrsach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kyrsach.Controllers
{
    public class TestsController : Controller
    {
        private readonly SerovaContext _context;

        public TestsController(SerovaContext context)
        {
            _context = context;
        }

        // GET: Tests
        public async Task<IActionResult> Index(int? idCat)
        {
            try { 
            ViewBag.idCat = idCat;
            var serovaContext =await _context.Tests.Include(t => t.IdCategoryNavigation).Where(t => t.IdCategory == idCat).ToListAsync();
            if (_context.Category.FirstOrDefault(c => c.IdCategory == idCat).WhoCreatedCategory == User.Claims.FirstOrDefault(u => u.Type == "id").Value)
                ViewBag.isThisUserCreated = true;
            return View( serovaContext);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }


        // GET: Tests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try { 
            if (id == null || _context.Tests == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .Include(t => t.IdCategoryNavigation)
                .FirstOrDefaultAsync(m => m.IdTest == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }

        // GET: Tests/Create
        public IActionResult Create(int idCat)
        {
            ViewBag.idCat = idCat;
            ViewData["IdCategory"] = new SelectList(_context.Category, "IdCategory", "IdCategory");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTest,IdCategory,NameTest")] Test test)
        {
            try { 
            if (ModelState.IsValid)
            {
                _context.Add(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { idCat = test.IdCategory });
            }
            ViewBag.idCat =  test.IdCategory;
            return View(test);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }

        // GET: Tests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try { 
            if (id == null || _context.Tests == null)
            {
                    return RedirectToAction("Index", "Errors", new { message = "Тест не найден" });
                }

                var test = await _context.Tests.FindAsync(id);
            ViewBag.idCat = test.IdCategory;
            if (test == null)
            {
                    return RedirectToAction("Index", "Errors", new { message="Тест не найден" });
                }
                ViewBag.idCat = test.IdCategory; return View(test);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTest,IdCategory,NameTest")] Test test)
        {
            if (ModelState.IsValid)
            {
                if (id != test.IdTest) return NotFound(); 
                try
                {
                    _context.Update(test);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Errors", new { ex.Message });
                }
                ViewBag.idCat = test.IdCategory;
                return RedirectToAction(nameof(Index), new { idCat = test.IdCategory });
            }
            ViewBag.idCat = test.IdCategory;
            return View(test);
        }

        // GET: Tests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null || _context.Tests == null) return NotFound();
                var test = await _context.Tests
                    .Include(t => t.IdCategoryNavigation)
                    .FirstOrDefaultAsync(m => m.IdTest == id);
                ViewBag.idCat = test.IdCategory;
                if (test == null) return NotFound();
                return View(test);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }

        }
            [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try { 
            if (_context.Tests == null)  return Problem("Entity set 'SerovaContext.Tests'  is null.");  
            var test = await _context.Tests.FindAsync(id);
            int idCat = test.IdCategory;
            ViewBag.idCat = test.IdCategory;
            if (test != null)  _context.Tests.Remove(test);  
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { idCat = idCat });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }

        private bool TestExists(int id)
        {
            return (_context.Tests?.Any(e => e.IdTest == id)).GetValueOrDefault();
        }
    }
}
