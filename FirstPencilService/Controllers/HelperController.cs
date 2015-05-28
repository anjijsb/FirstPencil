using Newtonsoft.Json;
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


        /// <summary>
        /// 通过Code获取用户OpenId
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetOpenidByCode(string code)
        {
            //var a = new Helper();
            //  a.WriteTxt(code);
            string openid = "err";

            string apps = System.Configuration.ConfigurationManager.AppSettings["appsecrect"];
            string appid = System.Configuration.ConfigurationManager.AppSettings["appid"];
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appid, apps, code);

            string resStr = WechatHelper.GetResponse("", url);

            // a.WriteTxt(resStr);

            //  resStr = string.Format("{{\"res\":{0} }}", resStr);
            var resXml = JsonConvert.DeserializeXNode(resStr, "res");
            var node = resXml.Element("res").Element("openid");
            if (node != null)
            {
                openid = node.Value; ;
            }
            else
            {
                openid = resStr;
            }
            return openid;
        }

        /// <summary>
        /// 获得js ticket
        /// </summary>
        /// <param name="url"></param>
        /// <param name="noncestr"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetJsApiSignature(string url, string noncestr, string timestamp)
        {
            var tic = WechatHelper.GetJsApiTicket();
            var cpyString = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", tic, noncestr, timestamp, url);
            var ret = WechatHelper.GetSha1Str(cpyString);
            return ret;
        }

    }
}
