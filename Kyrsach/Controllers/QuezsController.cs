using Kyrsach.Models;
using Kyrsach.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kyrsach.Controllers
{
    public class QuezsController : Controller
    {
        private readonly SerovaContext _context;

        public QuezsController(SerovaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int idQuez, int idT, string? answer)
        {
            try
            {
                ViewBag.idQuez = _context.Quezs.FirstOrDefault(t => t.IdTest == idT)?.IdQuez;
                int idCat = _context.Tests.FirstOrDefault(q => q.IdTest == idT).IdCategory;
                if ( _context.Category.FirstOrDefault(c=>c.IdCategory== idCat).WhoCreatedCategory == User.Claims.FirstOrDefault(u => u.Type == "id").Value)
                    ViewBag.isThisUserCreated = true;
                if (_context.Quezs.Where(t => t.IdTest == idT).FirstOrDefault() != null)
                {
                    ViewBag.idT = idT;
                    var serovaContext = _context.Quezs.Where(t => t.IdTest == idT);
                    ViewBag.Button = "Далее";
                    int[] numbers = new int[5];
                    Random rand = new Random();
                    for (int i = 0; i < numbers.Length - 1; i++)
                    {
                        int newNumber;
                        do
                        {
                            newNumber = rand.Next(5);
                        } while (Array.IndexOf(numbers, newNumber) != -1);
                        numbers[i] = newNumber;
                    }
                    ViewBag.numbers = numbers;
                    return View(await serovaContext.ToListAsync());
                }
                else
                {
                    ViewBag.idT = idT;
                    var serovaContext = _context.Quezs.Where(t => t.IdTest == idT);
                    int[] numbers = new int[5];
                    Random rand = new Random();

                    for (int i = 0; i < numbers.Length - 1; i++)
                    {
                        int newNumber;

                        do
                        {
                            newNumber = rand.Next(5);
                        } while (Array.IndexOf(numbers, newNumber) != -1);

                        numbers[i] = newNumber;
                    }

                    ViewBag.numbers = numbers;
                    return View(await serovaContext.ToListAsync());
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }
        

        public async Task<IActionResult> Dalee(int idQues, int idT, string? answer)
        {
            try {
                int idCat = _context.Tests.FirstOrDefault(q => q.IdTest == idT).IdCategory;
                if (_context.Category.FirstOrDefault(c => c.IdCategory == idCat).WhoCreatedCategory == User.Claims.FirstOrDefault(u => u.Type == "id").Value)
                    ViewBag.isThisUserCreated = true;
                var c1 = _context.Quezs.OrderBy(t => t.IdQuez).LastOrDefault(t => t.IdTest == idT && t.IdQuez > idQues);
            var c2 = _context.Quezs.Where(t => t.IdTest == idT).FirstOrDefault(t => t.IdQuez > idQues);
            var c3 = c2 != null ? _context.Quezs.Where(t => t.IdTest == idT).FirstOrDefault(t => t.IdQuez > c2.IdQuez) : null;
            if (c3 != null && c2.IdQuez < c1.IdQuez)
            {
                ViewBag.idT = idT;
                int Id = c2.IdQuez;
                var serovaContext = _context.Quezs.Where(t => t.IdTest == idT);
                ViewBag.idQuez = Id;
                ViewBag.Button = "Далее";

                int[] numbers = new int[5];
                Random rand = new Random();

                for (int i = 0; i < numbers.Length - 1; i++)
                {
                    int newNumber;

                    do
                    {
                        newNumber = rand.Next(5);
                    } while (Array.IndexOf(numbers, newNumber) != -1);

                    numbers[i] = newNumber;
                }

                ViewBag.numbers = numbers;
                return View(nameof(Index), await serovaContext.ToListAsync());
            }
            else if (c1 != null)
            {
                ViewBag.idT = idT;
                var serovaContext = _context.Quezs.Where(t => t.IdTest == idT);
                ViewBag.IdQuez = c2.IdQuez;
                ViewBag.Button = "Готово";
                int[] numbers = new int[5];
                Random rand = new Random();
                for (int i = 0; i < numbers.Length - 1; i++)
                {
                    int newNumber;
                    do
                    {
                        newNumber = rand.Next(5);
                    } while (Array.IndexOf(numbers, newNumber) != -1);

                    numbers[i] = newNumber;
                }
                ViewBag.numbers = numbers;
                return View(nameof(Index), await serovaContext.ToListAsync());
            }
            else
            {
                return RedirectToAction(nameof(End));
            }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }

        public async Task<IActionResult> End()
        {
            return View();
        }


[Authorize(Roles = "teacher,admin")]
        public IActionResult Create(int idT)
        {
            ViewBag.idT = idT;
            ViewData["IdTest"] = new SelectList(_context.Tests, "IdTest", "IdTest");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "teacher,admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTest,IdQuez,Pic1,Pic2,Pic3,Pic4,Pic5,Q1,Q2,Q3,Q4,Q5")] QuezViewModel quezViewModel)
        {
            try
            {
                Quez tast = new Quez
                {
                    IdTest = quezViewModel.IdTest,
                    Q1 = quezViewModel.Q1,
                    Q2 = quezViewModel.Q2,
                    Q3 = quezViewModel.Q3,
                    Q4 = quezViewModel.Q4,
                    Q5 = quezViewModel.Q5
                };
                if (quezViewModel.Pic1 != null && quezViewModel.Pic2 != null && quezViewModel.Pic3 != null && 
                    quezViewModel.Pic4 != null && quezViewModel.Pic5 != null)
                {
                    byte[] imageData1 = null;
                    byte[] imageData2 = null;
                    byte[] imageData3 = null;
                    byte[] imageData4 = null;
                    byte[] imageData5 = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(quezViewModel.Pic1.OpenReadStream()))
                    {
                        imageData1 = binaryReader.ReadBytes((int)quezViewModel.Pic1.Length);
                    }
                    // установка массива байтов
                    tast.Pic1 = imageData1;

                    using (var binaryReader = new BinaryReader(quezViewModel.Pic2.OpenReadStream()))
                    {
                        imageData2 = binaryReader.ReadBytes((int)quezViewModel.Pic2.Length);
                    }
                    // установка массива байтов
                    tast.Pic2 = imageData2;

                    using (var binaryReader = new BinaryReader(quezViewModel.Pic3.OpenReadStream()))
                    {
                        imageData3 = binaryReader.ReadBytes((int)quezViewModel.Pic3.Length);
                    }
                    // установка массива байтов
                    tast.Pic3 = imageData3;

                    using (var binaryReader = new BinaryReader(quezViewModel.Pic4.OpenReadStream()))
                    {
                        imageData4 = binaryReader.ReadBytes((int)quezViewModel.Pic4.Length);
                    }
                    // установка массива байтов
                    tast.Pic4 = imageData4;

                    using (var binaryReader = new BinaryReader(quezViewModel.Pic5.OpenReadStream()))
                    {
                        imageData5 = binaryReader.ReadBytes((int)quezViewModel.Pic5.Length);
                    }
                    // установка массива байтов
                    tast.Pic5 = imageData5;
                }

                _context.Add(tast);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { idT = tast.IdTest });

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }



[Authorize(Roles = "teacher,admin")]
        public async Task<IActionResult> Delete(int id, int idT)
        {
            try{ 
                if (_context.Quezs == null)
            {
                return Problem("Соотношений нет");
            }
            var quez = await _context.Quezs.FindAsync(id);
            if (quez != null)
            {
                _context.Quezs.Remove(quez);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { idT = quez.IdTest });
        }
            catch(Exception ex) 
            {
                return RedirectToAction("Index", "Errors", new { ex.Message
    });
            }
        }

        // POST: Quezs/Delete/5


        private bool QuezExists(int id)
        {
            return (_context.Quezs?.Any(e => e.IdQuez == id)).GetValueOrDefault();
        }
    }
}
