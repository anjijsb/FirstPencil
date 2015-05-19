using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstPencilService.Controllers
{
    public class HelperController : ApiController
    {
        /// <summary>
        /// 获取accesstoken
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetAccessToken()
        {
            return WechatHelper.GetToken();
        }
    }
}
