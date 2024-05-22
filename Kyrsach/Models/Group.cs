using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public partial class Group
    {
        [Key]
        public int IdGroup { get; set; }
        public string NameGroup { get; set; } = null!;


    }
}
