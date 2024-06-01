using Kyrsach.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.ViewModels
{
    public partial class GroupViewModel
    {
        public int IdGroup { get; set; }
        [RegularExpression(@"^[а-яА-Я0-9,\.\s-]{1,10}$", ErrorMessage = "Некорректные данные. Поле должно содержать от 1 до 10 русских символов, цифр, запятых, тире и точек")]
        [StringLength(10, MinimumLength = 1)]
        public string NameGroup { get; set; } = null!;
        public bool isIn { get; set; }
        public virtual List<UserModel> Students { get; set; }

    }
}
