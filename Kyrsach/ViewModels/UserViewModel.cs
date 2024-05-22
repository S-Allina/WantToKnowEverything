using Microsoft.AspNetCore.Identity;

namespace Kyrsach.Models
{
    public class UserViewModel : IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? NumberStudentBook { get; set; }
        public string? NameGroup { get; set; }
        public string NameRole { get; set; }
    }
}
