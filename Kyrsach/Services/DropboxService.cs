using Dropbox.Api.Files;
using Dropbox.Api;
using Kyrsach.Services.IServices;
using Kyrsach.Models;
using Kyrsach.ViewModels;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace Kyrsach.Services
{
    public class DropboxService : IDropboxService
    {
        private  DropboxClient _dropboxClient;
        public DropboxService( IConfiguration configuration)
        {
            var t = configuration.GetSection("DropboxOptions").Get<DropboxOptions>();
            _dropboxClient = new DropboxClient(t.AccessToken);
        }

        public async Task<FileModel> UploadFileAsync(IFormFile file)
        {        
            byte[] fileBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            var mem = new MemoryStream(fileBytes);
            // Использование существующего экземпляра _dropboxClient
            var uploadResponse = await _dropboxClient.Files.UploadAsync($"/{file.FileName}", WriteMode.Overwrite.Instance, body: mem);
            return new FileModel { Path = uploadResponse.PathDisplay, Name = uploadResponse.Name };
        }

        public async Task<FileViewModel> DownloadFileAsync(string filePath)
        {
                // Логика скачивания файла из Dropbox
                var downloadResponse = await _dropboxClient.Files.DownloadAsync(filePath);
                var fileContents = await downloadResponse.GetContentAsByteArrayAsync();
                return new FileViewModel { Data = fileContents, Name = Path.GetFileName(filePath) };
        }
        public async Task DeleteFileAsync(string filePath)
        {
            await _dropboxClient.Files.DeleteV2Async(filePath);
        }
    }

}
