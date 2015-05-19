using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPencilService.Models
{
    /// <summary>
    /// 投诉信息
    /// </summary>
    public class Complain
    {
        [Key]
        public int ComplainId { get; set; }

        public DateTime CreateDate { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Address { get; set; }

        public string Remarks { get; set; }

        public string ImgId { get; set; }

        public string ImgPath { get; set; }


    }
}
