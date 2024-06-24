using Kyrsach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;
using Dropbox.Api;
using Dropbox.Api.Files;
using System.Configuration;
using static Dropbox.Api.Files.SearchMatchType;
using System.Text;
using Kyrsach.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Kyrsach.Controllers
{
    public class FilesController : Controller
    {
        private readonly IDropboxService _dropboxService;
        private readonly SerovaContext _context;

        public FilesController(IDropboxService dropboxService, SerovaContext context)
        {
            _dropboxService = dropboxService;
            _context = context;
        }
        [Authorize(Roles = "teacher,admin")]
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
        [Authorize(Roles ="teacher,admin")]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, string Name)
        {
            try
            {
                if (file != null && Name != null)
                {
                    var uploadResult = await _dropboxService.UploadFileAsync(file);
                    await SaveFilePathToDatabase(uploadResult.Path, file, Name);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.message = "Не все поля заполнены";
                    return View();
                }
            }
            catch (Exception ex)
            {
				ViewBag.message = ex.Message;
				return View();
			}
        }
        [Authorize]
        public IActionResult Index()
        {
            ViewBag.idCurrentUser = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
           var files= _context.Files?.ToList();
            return View(files);
        }
        [Authorize]
        public async Task<IActionResult> Download(int id)
        {
            var fileUrl = _context.Files.FirstOrDefault(f => f.id == id).Path;
            var downloadResult = await _dropboxService.DownloadFileAsync(fileUrl);

            var memoryStream = new MemoryStream(downloadResult.Data);

            return File(memoryStream, "application/octet-stream", downloadResult.Name);
        }




        public async Task SaveFilePathToDatabase(string path, IFormFile file, string Name)
        {
            var fileModel = new FileModel { Name = Name, Path = path, WhoCreated= User.Claims.FirstOrDefault(u => u.Type == "id").Value };
            await _context.Files.AddAsync(fileModel);
            await _context.SaveChangesAsync();
        }
        [Authorize(Roles = "teacher,admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var fileUrl = _context.Files.FirstOrDefault(f => f.id == id).Path;
                await _dropboxService.DeleteFileAsync(fileUrl);
                await DeleteFileFromDatabase(fileUrl);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting file from Dropbox and database.");
            }
        }

        private async Task DeleteFileFromDatabase(string filePath)
        {
            var file = await _context.Files.FirstOrDefaultAsync(f => f.Path == filePath);
            if (file != null)
            {
                _context.Files.Remove(file);
                await _context.SaveChangesAsync();
            }
        }

    }
}
