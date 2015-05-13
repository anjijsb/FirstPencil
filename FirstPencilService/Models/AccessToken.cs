
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPencilService.Models
{


    public class AccessToken
    {
        [Key]
        public int AcceccTokenId { get; set; }

        public string Token { get; set; }

        public DateTime? GetTime { get; set; }

    }
}
