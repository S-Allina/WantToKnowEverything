
using Kyrsach.Models;
using Kyrsach.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Controllers
{
    public class PairsController : Controller
    {
        private readonly SerovaContext _context;

        public PairsController(SerovaContext context)
        {
            _context = context;
        }

        // GET: Pairs
        public async Task<IActionResult> Index(int idCat)
        {

			ViewBag.idCat = idCat;
			//ViewBag.type = _context.Category.First(c => c.IdCategory == idCat).Type;
   //         var cards = _context.Pairs.Where(c => c.IdCategory == idCat).Distinct();

			return View();
        }


        [HttpGet]
        public async Task<IActionResult> IndexJSON(int idCat)
        {

            ViewBag.idCat = idCat;
            ViewBag.type = _context.Category.First(c => c.IdCategory == idCat).Type;
            var cards = _context.Pairs.Where(c => c.IdCategory == idCat).Distinct();
            List<ObjectJson> obj = new List<ObjectJson>();
            foreach (var i in cards)
            {
                obj.Add(new ObjectJson
                {
                    Card1Text = i.Card1Text,
                    Card2Text = i.Card2Text,
                    Card1Img = i.Card1Img==null? null : Convert.ToBase64String(i.Card1Img),
                    Card2Img = i.Card2Img == null ? null : Convert.ToBase64String(i.Card2Img),
                });
            }
                return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> ListPairs(int idCat)
        {
            ViewBag.idCat = idCat;
            ViewBag.type = _context.Category.First(c => c.IdCategory == idCat).Type;
            return View(_context.Pairs.Where(p => p.IdCategory == idCat));
        }



[Authorize(Roles = "teacher,admin")]
        [HttpGet]
        public IActionResult Create(int idCat)
        {
            ViewBag.idCat = idCat;
            ViewBag.type = _context.Category.First(p => p.IdCategory == idCat).Type;
            ViewData["IdCategory"] = new SelectList(_context.Category, "IdCategory", "IdCategory");
            return View();
        }

        [HttpPost]
[Authorize(Roles = "teacher,admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPair,IdCategory,Card1Text,Card2Text,Card1Img,Card2Img")] PairViewModel pairViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Pair pair = new Pair
                    {
                        IdPair = pairViewModel.IdPair,
                        IdCategory = pairViewModel.IdCategory,
                        Card1Text = pairViewModel.Card1Text,
                        Card2Text = pairViewModel.Card2Text

                    };


                    if (pairViewModel.Card2Img != null)
                    {
                        byte[] imageData = null;

                        // считываем переданный файл в массив байтов
                        using (var binaryReader = new BinaryReader(pairViewModel.Card2Img.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)pairViewModel.Card2Img.Length);
                        }
                        // установка массива байтов
                        pair.Card2Img = imageData;

                    }

                    _context.Add(pair);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("ListPairs", new { idCat = pairViewModel.IdCategory });
                }
                else
                {
                    return View(pairViewModel);

                }
            }
            catch
            {
                return View(pairViewModel);
            }
        }

        // GET: Pairs/Edit/5
        //public async Task<IActionResult> Edit(int? idPairs)
        //{
        //    if (idPairs == null || _context.Pairs == null)
        //    {
        //        return NotFound();
        //    }

        //    var pair = await _context.Pairs.FindAsync(idPairs);
        //    if (pair == null)
        //    {
        //        return NotFound();
        //    }
        //    //ViewData["IdField"] = new SelectList(_context.Fields, "IdField", "IdField", pair.IdField);
        //    return View(pair);
        //}

        // POST: Pairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

[Authorize(Roles = "teacher,admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idPairs, [Bind("IdPair,IdCategory,Card1Text,Card2Text,Card1Img,Card2Img")] Pair pair)
        {

            try
            {
                _context.Update(pair);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListPairs), new { idCat = pair.IdCategory });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PairExists(pair.IdPair))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
        }

[Authorize(Roles = "teacher,admin")]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int idPairs)
        {
            if (_context.Pairs == null)
            {
                return Problem("Entity set 'SerovaContext.Pairs'  is null.");
            }
            var pair = await _context.Pairs.FindAsync(idPairs);
            int idCat = pair.IdCategory;
            if (pair != null)
            {
                _context.Pairs.Remove(pair);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ListPairs", new { idCat = idCat });
        }

        private bool PairExists(int id)
        {
            return (_context.Pairs?.Any(e => e.IdPair == id)).GetValueOrDefault();
        }
    }
    public class ObjectJson
    {
		public string Card1Text {get;set;} 
        public string Card2Text  { get; set;}
    public string Card1Img { get; set; }
        public string Card2Img { get; set; }
    }
}
