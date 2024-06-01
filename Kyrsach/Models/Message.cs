using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public partial class Message
    {
        [Key]
        public int IdMessage { get; set; }
        public int IdGroup { get; set; }
        public string IdSender { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTime Time { get; set; }


    }
}
