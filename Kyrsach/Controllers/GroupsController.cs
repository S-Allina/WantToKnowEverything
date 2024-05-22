using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kyrsach.Models;
using Kyrsach.ViewModels;
using Microsoft.AspNetCore.Identity;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Kyrsach.Controllers
{
    public class GroupsController : Controller
    {
        private readonly SerovaContext _context;

        public GroupsController(SerovaContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index(int? idGroup)
        {
            var userId = User.Claims.Where(u => u.Type == "id")?.FirstOrDefault()?.Value;
            var groups = await _context.Groups.ToListAsync();
            List<GroupViewModel> groupViewModel = new List<GroupViewModel>();
            foreach(var g in groups)
            {
                bool isInGroup = _context.PeopleInGroups.FirstOrDefault(p => p.IdGroup == g.IdGroup && p.IdUser == userId) != null;
                groupViewModel.Add(new GroupViewModel
                {
                    IdGroup=g.IdGroup,
                    NameGroup=g.NameGroup,
                    isIn = isInGroup,
                    Students = (from u in _context.Users 
                                join pg in _context.PeopleInGroups on u.Id equals pg.IdUser
                                where pg.IdGroup==g.IdGroup && pg.Role!="admin"
                                select u).ToList(),
                });
            }
            if (idGroup != null)
            {
                Group group = await _context.Groups.FindAsync(idGroup);
                ViewBag.Id = idGroup;
                ViewBag.name=group.NameGroup;
            }
            return View(groupViewModel);
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .FirstOrDefaultAsync(m => m.IdGroup == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }
        public async Task<IActionResult> Follow(int id)
        {
            var userId = User.Claims.Where(u => u.Type == "id")?.FirstOrDefault()?.Value;
            if (userId != null)
            {
                PeopleInGroup peopleInGroup = new PeopleInGroup
                {
                    IdGroup = id,
                    IdUser= userId,
                    Role="teacher"
                };
                await _context.PeopleInGroups.AddAsync(peopleInGroup);
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Unfollow(int id)
        {
            var userId = User.Claims.Where(u => u.Type == "id")?.FirstOrDefault()?.Value;
            if (userId != null)
            {
                PeopleInGroup peopleInGroup = await _context.PeopleInGroups.FirstOrDefaultAsync(p=>p.IdGroup==id && p.IdUser==userId);
                 _context.PeopleInGroups.Remove(peopleInGroup);
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
        // GET: Groups/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string NameGroup)
        {
            if (NameGroup != null)
            {
                Group group = new Group
                {
                    NameGroup = NameGroup,
                };
                PeopleInGroup peopleInGroup = new PeopleInGroup
                {
                    IdGroup = _context.Groups.Select(g=>g.IdGroup).Max(),
                    IdUser = _context.UserView.FirstOrDefault(u=>u.NameRole=="admin").Id,
                    Role = "admin"

                };
                await _context.PeopleInGroups.AddAsync(peopleInGroup);
                _context.SaveChanges();
                _context.Add(group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Не указано имя.");
            return View(NameGroup);
        }

        // GET: Groups/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Groups == null)
        //    {
        //        return NotFound();
        //    }

        //    Group group = await _context.Groups.FindAsync(id);
        //    if (group == null)
        //    {
        //        return NotFound();
        //    }
        //    return RedirectToAction("Index",new { group });
        //}

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int IdGroup, string NameGroup)
        {
            try
            {
                if (IdGroup != 0 && NameGroup != null)
                {

                    var group = _context.Groups.FirstOrDefault(g => g.IdGroup == IdGroup);
                    group.NameGroup = NameGroup;
                    _context.Update(group);
                    await _context.SaveChangesAsync();

                    //catch (DbUpdateConcurrencyException)
                    //{
                    //    if (!GroupExists(id))
                    //    {
                    //        return NotFound();
                    //    }
                    //    else
                    //    {
                    //        throw;
                    //    }
                    //}
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Index", "Errors");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }

      
        // POST: Groups/Delete/5
        [HttpGet, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Groups == null)
            {
                return Problem("Entity set 'SerovaContext.Groups'  is null.");
            }
            var @group = await _context.Groups.FindAsync(id);
            if (@group != null)
            {
                _context.Groups.Remove(@group);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
          return (_context.Groups?.Any(e => e.IdGroup == id)).GetValueOrDefault();
        }
    }
}
