using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstPencilWeb.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string OpenId { get; set; }

        public string NickName { get; set; }

        public bool subscribe { get; set; }

        public int Sex { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Province { get; set; }

        public string Language { get; set; }

        public DateTime? SubscribeTime { get; set; }

        public string Headimgurl { get; set; }

        public bool IsSalesman { get; set; }

        public int? SalesmanId { get; set; }

        public string Salesman { get; set; }

        public string Remarks { get; set; }
    }
}