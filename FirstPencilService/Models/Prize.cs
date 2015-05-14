using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPencilService.Models
{
    public class Prize
    {
        [Key]
        public int PrizeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Remarkes { get; set; }
    }

}
