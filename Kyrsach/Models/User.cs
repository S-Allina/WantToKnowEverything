using Microsoft.AspNetCore.Identity;

namespace Kyrsach.Models
{
    public class UserModel : IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? NumberStudentBook { get; set; }
    }
}
