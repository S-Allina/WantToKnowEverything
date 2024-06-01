using System.ComponentModel.DataAnnotations;

namespace Kyrsach.ViewModels
{
    public class FileViewModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public byte[]? Data { get; set; }
        public string WhoCreated { get; set; }

    }
}
