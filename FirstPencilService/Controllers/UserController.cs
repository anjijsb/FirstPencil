using FirstPencilService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstPencilService.Controllers
{
    public class UserController : ApiController
    {
        /// <summary>
        /// 通过openid获取用户详细信息
        /// </summary>
        /// <param name="openid">用户openid</param>
        /// <returns>user模型，若不存在返回null</returns>
        [HttpGet]
        public User GetUserInfoByOpenid(string openid)
        {
            var db = new ModelContext();
            return db.UserSet.Include("Salesman").FirstOrDefault(u => u.OpenId == openid);
        }


    }
}
