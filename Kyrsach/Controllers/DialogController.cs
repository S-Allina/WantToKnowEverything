
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Kyrsach.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Kyrsach.ViewModels;

namespace Kyrsach.Controllers
{
    public class DialogController : Controller
    {
        private readonly SerovaContext _context;
        private readonly UserManager<UserModel> _userManager;


        public DialogController(SerovaContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> DialogUser()
        {
            var IdUser = _userManager.GetUserAsync(HttpContext.User);
            if (User.IsInRole("user"))
            {
                var groupId = _context.PeopleInGroups.FirstOrDefault(p => p.IdUser == IdUser.Result.Id).IdGroup;
                //var group = await _context.Groups.FirstOrDefaultAsync(u => u.IdGroup == groupId);

                return RedirectToAction("IndexDialog", new { IdGroup= groupId });
            }
            else
            {
                var groups = await _context.Groups
       .Join(
           _context.PeopleInGroups
               .Where(d => d.IdUser == IdUser.Result.Id)
               .Select(d => d.IdGroup)
               .Distinct(),
           u => u.IdGroup,
           userId => userId,
           (u, userId) => u
       )
       .ToListAsync();
                List<GroupViewModel> groupViewModel = new List<GroupViewModel>();
                foreach (var g in groups)
                {
                    groupViewModel.Add(new GroupViewModel
                    {
                        IdGroup = g.IdGroup,
                        NameGroup = g.NameGroup,
                        Students = (from u in _context.Users
                                    join pg in _context.PeopleInGroups on u.Id equals pg.IdUser
                                    where pg.IdGroup == g.IdGroup && pg.Role != "admin"
                                    select u).ToList(),
                    });
                }
                return View(groupViewModel);
            } 
        }

        [Authorize]
        public async Task<IActionResult> IndexDialog(int IdGroup)
        {
            var group =await _context.Groups.FirstOrDefaultAsync(u=>u.IdGroup== IdGroup);
            return View(group);
        }
       
    }
}
