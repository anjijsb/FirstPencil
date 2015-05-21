using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPencilService.Models
{
    public class Attention
    {
        [Key]
        public int AttentionId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Remarks { get; set; }

        public DateTime? CreateDate { get; set; }

        public bool IsStick { get; set; }

        public bool IsActive { get; set; }

    }

    public class AttentionRegister
    {
        [Key]
        public int AttentionRegisterId { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        virtual public User User { get; set; }

        public int? AttentionId { get; set; }

        [ForeignKey("AttentionId")]
        virtual public Attention Attention { get; set; }

        public DateTime? CreateDate { get; set; }

        public string Remarks { get; set; }


    }


}
