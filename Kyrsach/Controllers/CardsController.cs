using Microsoft.AspNetCore.Mvc;

namespace Kyrsach.Controllers
{
    public class CardsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CardsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile pdfFile)
        {



            if (pdfFile != null && pdfFile.Length > 0)
            {

                var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "img", pdfFile.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(fileStream);
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(fileBytes, "application/pdf");

            }
            else
            {
                return BadRequest("Please select a PDF file.");
            }
        }
    }
}