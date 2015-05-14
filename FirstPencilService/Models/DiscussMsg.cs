using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPencilService.Models
{
    public class DiscussMsg
    {
        [Key]
        public int MsgId { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public string Content { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
