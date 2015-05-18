using FirstPencilService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstPencilService.Controllers
{
    public class ComplainController : ApiController
    {
        /// <summary>
        /// 添加打假信息
        /// </summary>
        /// <param name="address">事发地</param>
        /// <param name="title">标题、简要描述<</param>
        /// <param name="content">详细内容</param>
        /// <param name="openid">用户openid</param>
        /// <returns></returns>
        [HttpPost]
        public bool AddComplain(string title, string address, string content, string openid)
        {
            var db = new ModelContext();
            var user = db.UserSet.FirstOrDefault(u => u.OpenId == openid);
            if (user == null)
            {
                return false;
            }
            if (user != null)
            {

                ;
            }
            var com = new Complain
            {
                UserId = user.UserId,
                Address = address,
                Content = content,
                Title = title,
                CreateDate = DateTime.Now,
            };

            db.ComplainSet.Add(com);
            db.SaveChanges();
            return true;
        }
    }
}
