using Microsoft.AspNetCore.Mvc;
namespace Kyrsach.Controllers
{
    public class BiographyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Start()
        {
            return View();
        }
        public IActionResult Speak()
        {
            return View();
        }
    }
}
