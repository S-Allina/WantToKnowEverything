using DocumentFormat.OpenXml.Spreadsheet;
using Humanizer;
using Kyrsach.Models;
using Kyrsach.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kyrsach.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly SerovaContext _context;
        public AccountController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, SerovaContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userFromNumber = _userManager.Users.Where(u => u.FirstName == model.FirstName
                    && u.LastName == model.LastName && u.NumberStudentBook == model.NumberStudentBook).FirstOrDefault();
                    if (userFromNumber == null) ModelState.AddModelError("", "Студента с таким номером зачётки ещё не зарегистрировано. Проверьте правильность введённых данных.");
                    else
                    {
                        if (_userManager.FindByNameAsync(model.Name).Result != null)
                        {
                            ModelState.AddModelError(string.Empty, "Логин не уникален");
                        }
                        if (_userManager.FindByEmailAsync(model.Email).Result != null)
                        {
                            ModelState.AddModelError(string.Empty, "Email не уникален");

                        }
                        if (_context.Users.FirstOrDefault(u => u.NumberStudentBook == model.NumberStudentBook).Email != null) ModelState.AddModelError(string.Empty, "Ученик с этим номером зачётки уже зарегистрирован");
                        if (ModelState.ErrorCount != 0) return View(model);
                        userFromNumber.Email = model.Email;
                        userFromNumber.UserName = model.Name;
                        var result = _userManager.PasswordHasher.HashPassword(userFromNumber, model.Password);
                        userFromNumber.PasswordHash = result;
                        await _userManager.UpdateAsync(userFromNumber);
                        return RedirectToAction("Index", "Home");

                    }
                }
                return View(model);
            }catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> FullUsers()
        {
            try
            {
                var user = _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result;
                List<UserViewModel> users = new List<UserViewModel>();
                ViewBag.Groups = await GetGroupsSelectList();

                if (await _userManager.IsInRoleAsync(user, "admin"))
                {
                    users = _context.UserView.Where(u=>u.Id != user.Id).OrderBy(u => u.NameGroup).ToList();
                    return View(users);
                }
                else if (await _userManager.IsInRoleAsync(user, "teacher"))
                {
                    users = _context.UserView.Where(u => u.NameRole == "user").OrderBy(u => u.NameGroup).ToList();
                    return View(users);
                }
                return RedirectToAction("Index", "Home");
              }catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message
    });
            }
        }

        private async Task<List<SelectListItem>> GetGroupsSelectList()
        {
            var groups = await _context.Groups.ToListAsync();
            var selectListItems = groups.Select(c => new SelectListItem
            {
                Value = c.IdGroup.ToString(),
                Text = c.NameGroup
            }).ToList();

            return selectListItems;
            
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RegisterTeacher()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "teacher,admin")]
        public async Task<IActionResult> RegisterUser()
        {
            ViewBag.Groups = await GetGroupsSelectList();
            return View();
        }

       

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterTeacher(RegisterModel model)
        {
            try { 
            if (ModelState.IsValid)
            {
                if (_userManager.FindByNameAsync(model.Name).Result != null) ModelState.AddModelError(string.Empty, "Логин не уникален");
                if (_userManager.FindByEmailAsync(model.Email).Result != null)ModelState.AddModelError(string.Empty, "Email не уникален");
                if (ModelState.ErrorCount != 0) return View(model);
                UserModel user = new UserModel
                {
                    Email = model.Email,
                    UserName = model.Name,
                    NormalizedUserName = model.LastName + model.FirstName,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, "teacher");
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "teacher,admin")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserModel model)
        {
            try { 
            if (ModelState.IsValid)
            {
                if(_context.Users.FirstOrDefault(u=>u.NumberStudentBook==model.NumberStudentBook)!=null) ModelState.AddModelError(string.Empty, "Номер зачётки не уникален. Ученик с таким номером зачётки уже зарегистрирован.");
                if (ModelState.ErrorCount != 0) return View(model);

                UserModel user = new UserModel
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    NumberStudentBook = model.NumberStudentBook,
                    UserName = model.NumberStudentBook.ToString()

                };

                //var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, "user");
               var t = await _signInManager.CanSignInAsync(user);
                PeopleInGroup peopleInGroup = new PeopleInGroup
                {
                    IdGroup = (int)model.Group,
                    IdUser = _userManager.Users.FirstOrDefault(u=>u.NumberStudentBook==model.NumberStudentBook).Id,
                    Role = "user"

                };
                await _context.PeopleInGroups.AddAsync(peopleInGroup);
                _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
            }

            ViewBag.Groups = await GetGroupsSelectList();
            return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
		 await _signInManager.GetExternalAuthenticationSchemesAsync();
            return View(new LoginModel {});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try { 
            if (model.Name != null && model.Password != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.Name);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Index", "Home");
                    }
                    else ModelState.AddModelError("", "Неверный логин или пароль");
                }
                else ModelState.AddModelError("", "Пользователя с таким логином не существует, проверьте правильность введённых данных.");
            }
            else ModelState.AddModelError("", "Не все поля заполнены"); 
            return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { ex.Message });
            }
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        // Пользователь успешно удален
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        string errors = "";
                        // Произошла ошибка при удалении пользователя
                        foreach (var error in result.Errors)
                        {
                            errors += error.Description + " ";
                        }
                        return RedirectToAction("Index", "Errors", new { errors });

                    }
                }
                else
                {
                    // Пользователь не найден
                    return RedirectToAction("Index", "Errors", new { message = "такого пользователя нет в бд" });

                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Errors", new { message ="Нельзя удалить ученика так как его данные указаны в других таблицах" });

            }
        }
    }
}


