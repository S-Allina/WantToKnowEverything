using Kyrsach.Models;
using Kyrsach.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System;
using System.Diagnostics;
using ClosedXML.Excel;
using Kyrsach.ViewModels;
using DocumentFormat.OpenXml.Office.CustomXsn;

namespace Kyrsach.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register()
        {
            return RedirectToAction(nameof(Index));
        }
     

        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}