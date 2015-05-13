using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPencilService.Models
{
    public class Firm
    {
        [Key]
        public int FirmId { get; set; }

        public string Name { get; set; }

        public string Area { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Remarks { get; set; }
    }
    
    public class Salesman
    {
        [Key]
        public int SalesmanId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public Sex Sex { get; set; }

        public int? FirmId { get; set; }

        [ForeignKey("FirmId")]
        public virtual Firm Firm { get; set; }

        public string Position { get; set; }

        public string Remarks { get; set; }
    }
}
