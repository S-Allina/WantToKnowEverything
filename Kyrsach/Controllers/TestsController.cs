
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
            ViewBag.idCat = idCat;
            var serovaContext = _context.Tests.Include(t => t.IdCategoryNavigation).Where(t => t.IdCategory == idCat);
            return View(await serovaContext.ToListAsync());
        }


        // GET: Tests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: Tests/Create
        public IActionResult Create(int idCat)
        {
            ViewBag.idCat = idCat;
            ViewData["IdCategory"] = new SelectList(_context.Category, "IdCategory", "IdCategory");
            return View();
        }

        // POST: Tests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTest,IdCategory,NameTest,WhoAnsweredTest")] Test test)
        {
            _context.Add(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { idCat = test.IdCategory });

            ViewData["IdCategory"] = new SelectList(_context.Category, "IdCategory", "IdCategory", test.IdCategory);
            return View(test);
        }

        // GET: Tests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tests == null)
            {
                return NotFound();
            }

            var test = await _context.Tests.FindAsync(id);
            ViewBag.idCat = test.IdCategory;
            if (test == null)
            {
                return NotFound();
            }
            ViewData["IdCategory"] = new SelectList(_context.Category, "IdCategory", "IdCategory", test.IdCategory);
            return View(test);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTest,IdCategory,NameTest,WhoAnsweredTest")] Test test)
        {
            if (id != test.IdTest)
            {
                return NotFound();
            }

            try
            {
                _context.Update(test);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestExists(test.IdTest))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }


            }
            ViewBag.idCat = test.IdCategory;
            return RedirectToAction(nameof(Index), new { idCat = test.IdCategory });
            ViewData["IdCategory"] = new SelectList(_context.Category, "IdCategory", "IdCategory", test.IdCategory);
            return View(test);
        }

        // GET: Tests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tests == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .Include(t => t.IdCategoryNavigation)
                .FirstOrDefaultAsync(m => m.IdTest == id);
            ViewBag.idCat = test.IdCategory;
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tests == null)
            {
                return Problem("Entity set 'SerovaContext.Tests'  is null.");
            }

            var test = await _context.Tests.FindAsync(id);
            int idCat = test.IdCategory;
            ViewBag.idCat = test.IdCategory;
            if (test != null)
            {
                _context.Tests.Remove(test);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { idCat = idCat });
        }

        private bool TestExists(int id)
        {
            return (_context.Tests?.Any(e => e.IdTest == id)).GetValueOrDefault();
        }
    }
}
