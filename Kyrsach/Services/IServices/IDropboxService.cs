using Kyrsach.Models;
using Kyrsach.ViewModels;

namespace Kyrsach.Services.IServices
{
    public interface IDropboxService
    {
        Task<FileModel> UploadFileAsync(IFormFile file);
        Task<FileViewModel> DownloadFileAsync(string filePath);
        Task DeleteFileAsync(string filePath);
    }
}
