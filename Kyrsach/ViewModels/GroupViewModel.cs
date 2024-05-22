using Kyrsach.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.ViewModels
{
    public partial class GroupViewModel
    {
        public int IdGroup { get; set; }
        public string NameGroup { get; set; } = null!;
        public bool isIn { get; set; }
        public virtual List<UserModel> Students { get; set; }

    }
}
