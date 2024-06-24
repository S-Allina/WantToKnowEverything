
using DocumentFormat.OpenXml.Spreadsheet;
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
            try { 
			ViewBag.idCat = idCat;
            if (_context.Category.FirstOrDefault(c => c.IdCategory ==idCat).WhoCreatedCategory == User.Claims.FirstOrDefault(u => u.Type == "id").Value)
                ViewBag.isThisUserCreated = true;
            //ViewBag.type = _context.Category.First(c => c.IdCategory == idCat).Type;
            //         var cards = _context.Pairs.Where(c => c.IdCategory == idCat).Distinct();

            return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> IndexJSON(int idCat)
        {
            try { 
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
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListPairs(int idCat, string? message)
        {
            try {
                ViewBag.message = message;

			ViewBag.idCat = idCat;
            ViewBag.type = _context.Category.First(c => c.IdCategory == idCat).Type;
            return View(_context.Pairs.Where(p => p.IdCategory == idCat));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }



        [Authorize(Roles = "teacher,admin")]
        [HttpGet]
        public IActionResult Create(int idCat)
        {
            try { 
            ViewBag.idCat = idCat;
            ViewBag.type = _context.Category.First(p => p.IdCategory == idCat).Type;
            ViewData["IdCategory"] = new SelectList(_context.Category, "IdCategory", "IdCategory");
            return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
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
                    if ((pairViewModel.Card1Text!=null && pairViewModel.Card2Text!=null) ||(pairViewModel.Card2Img!=null && pairViewModel.Card2Text!=null)) {
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
                            using (var binaryReader = new BinaryReader(pairViewModel.Card2Img.OpenReadStream()))
                            {
                                imageData = binaryReader.ReadBytes((int)pairViewModel.Card2Img.Length);
                            }
                            pair.Card2Img = imageData;
                        }

                        _context.Add(pair);

                        await _context.SaveChangesAsync();
                        return RedirectToAction("ListPairs", new { idCat = pairViewModel.IdCategory });
                    }
                    else {
                        ModelState.AddModelError("","Не все поля заполнены.");
                        return View(pairViewModel);

                    }
                }
                else
                {
                    return View(pairViewModel);
                }
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("",ex.Message);
                return View(pairViewModel);
            }
        }

        [Authorize(Roles = "teacher,admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idPairs, [Bind("IdPair,IdCategory,Card1Text,Card2Text,Card1Img,Card2Img")] Pair pair)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(pair);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ListPairs), new { idCat = pair.IdCategory });
                }
                else
                {
					return RedirectToAction(nameof(ListPairs), new { idCat = pair.IdCategory,message= "Некорректные данные. Поле должно содержать от 3 до 15 русских символов, цифр, запятых, тире и точек" });

				}
			}
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message
    });
            }
        }

        [Authorize(Roles = "teacher,admin")]
        public async Task<IActionResult> Delete(int idPairs)
        {
            try { 
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
            return RedirectToAction("ListPairs", new { idCat });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
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
