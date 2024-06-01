using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public class FileModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string WhoCreated { get; set; }

        //public UserModel User { get; set; }
    }
}
