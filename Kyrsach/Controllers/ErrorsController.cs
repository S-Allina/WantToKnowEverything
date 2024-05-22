using Microsoft.AspNetCore.Mvc;

namespace Kyrsach.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult Index(string Message)
        {
            ViewBag.Message = Message;
            return View();
        }
    }
}
