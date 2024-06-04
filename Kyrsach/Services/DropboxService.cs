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

        public DropboxService(DropboxClient dropboxClient)
        {
            _dropboxClient = dropboxClient;
        }

        public async Task<FileModel> UploadFileAsync(IFormFile file)
        {        
            byte[] fileBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            _dropboxClient = new DropboxClient("sl.B2aQs36R3oIGW2RJ98W5F58SqiakYj3tGM6KvekK3wliaJKJvcVL_QBrWc22U-CvgfRm-8bPQFBY0hc-yJldS0mTQn4xAgT0IUQTJ3KKY8IWGHI8Qzo8loCps4rRGszpjP0waJcMONZNTGqAgYmBg1Y");
            var mem = new MemoryStream(fileBytes);
            // Использование существующего экземпляра _dropboxClient
            var uploadResponse = await _dropboxClient.Files.UploadAsync($"/{file.FileName}", WriteMode.Overwrite.Instance, body: mem);
            return new FileModel { Path = uploadResponse.PathDisplay, Name = uploadResponse.Name };
        }

        public async Task<FileViewModel> DownloadFileAsync(string filePath)
        {
                // Логика скачивания файла из Dropbox
                _dropboxClient = new DropboxClient("sl.B2aQs36R3oIGW2RJ98W5F58SqiakYj3tGM6KvekK3wliaJKJvcVL_QBrWc22U-CvgfRm-8bPQFBY0hc-yJldS0mTQn4xAgT0IUQTJ3KKY8IWGHI8Qzo8loCps4rRGszpjP0waJcMONZNTGqAgYmBg1Y");
                var downloadResponse = await _dropboxClient.Files.DownloadAsync(filePath);
                var fileContents = await downloadResponse.GetContentAsByteArrayAsync();
                return new FileViewModel { Data = fileContents, Name = Path.GetFileName(filePath) };
        }
        public async Task DeleteFileAsync(string filePath)
        {
            _dropboxClient = new DropboxClient("sl.B2aQs36R3oIGW2RJ98W5F58SqiakYj3tGM6KvekK3wliaJKJvcVL_QBrWc22U-CvgfRm-8bPQFBY0hc-yJldS0mTQn4xAgT0IUQTJ3KKY8IWGHI8Qzo8loCps4rRGszpjP0waJcMONZNTGqAgYmBg1Y");

            await _dropboxClient.Files.DeleteV2Async(filePath);
        }
    }

}
