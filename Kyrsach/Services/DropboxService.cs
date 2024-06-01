using Dropbox.Api.Files;
using Dropbox.Api;
using Kyrsach.Services.IServices;
using Kyrsach.Models;
using Kyrsach.ViewModels;

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
            _dropboxClient = new DropboxClient("sl.B2WX4uMKY5Xus3p8ATtn_U4gEae6_HYWGDwJb3gizYsiyfnt8xNz0JyrwrOqPOXFvV_J8cNRL62p35s6EGaOnEZpq6s2ZOA8b52K5T5mg67Q3_XzplrpElSfHS_qzVuqAWDVcwIbl181S6n55KFdmQ0");

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
                _dropboxClient = new DropboxClient("sl.B2WX4uMKY5Xus3p8ATtn_U4gEae6_HYWGDwJb3gizYsiyfnt8xNz0JyrwrOqPOXFvV_J8cNRL62p35s6EGaOnEZpq6s2ZOA8b52K5T5mg67Q3_XzplrpElSfHS_qzVuqAWDVcwIbl181S6n55KFdmQ0");

                var downloadResponse = await _dropboxClient.Files.DownloadAsync(filePath);
                var fileContents = await downloadResponse.GetContentAsByteArrayAsync();
                return new FileViewModel { Data = fileContents, Name = Path.GetFileName(filePath) };
            
        }
        public async Task DeleteFileAsync(string filePath)
        {
            _dropboxClient = new DropboxClient("sl.B2WX4uMKY5Xus3p8ATtn_U4gEae6_HYWGDwJb3gizYsiyfnt8xNz0JyrwrOqPOXFvV_J8cNRL62p35s6EGaOnEZpq6s2ZOA8b52K5T5mg67Q3_XzplrpElSfHS_qzVuqAWDVcwIbl181S6n55KFdmQ0");

            await _dropboxClient.Files.DeleteV2Async(filePath);
        }
    }

}
