using FirstPencilService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstPencilService.Controllers
{
    /// <summary>
    /// 大会现场用户互动接口
    /// </summary>
    public class DiscussMsgController : ApiController
    {
        /// <summary>
        /// 获得留言列表
        /// </summary>
        /// <param name="lastId">上次请求最大的id,若为初次请求则填写0</param>
        /// <returns>留言列表（包含用户信息）</returns>
        [HttpGet]
        public IEnumerable<DiscussMsg> GetMsgList(int lastId)
        {
            var db = new ModelContext();
            var ret = db.DiscussMsgSet.Include("User").Where(item => item.MsgId > lastId).AsEnumerable();
            return ret;
        }
    }
}
