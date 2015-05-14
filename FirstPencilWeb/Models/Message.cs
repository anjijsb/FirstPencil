using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstPencilWeb.Models
{
    public class Message
    {
        public User User { get; set; }
        public int MsgId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public string CreateDate { get; set; }
    }
}