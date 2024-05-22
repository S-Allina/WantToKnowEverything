using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public partial class PeopleInGroup
    {
        [Key]
        public int IdPeopleInGroup { get; set; }
        public string IdUser { get; set; } = null!;
        public int IdGroup { get; set; }
        public string Role { get; set; } = null!;

        //public virtual UserModel IdUserNavigation { get; set; } = null!;
    }
}
