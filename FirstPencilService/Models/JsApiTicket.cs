using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPencilService.Models
{
    class JsApiTicket
    {
        [Key]
        public int TicketId { get; set; }

        public string Ticket { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
