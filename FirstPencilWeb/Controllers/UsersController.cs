using FirstPencilWeb.Helps;
using FirstPencilWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FirstPencilWeb.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 经销商中心
        /// </summary>
        /// <returns></returns>
        public ActionResult Dealerships(string code)
        {
            WeiXinUserInfo info = new WeiXinUserInfo();
            if (!string.IsNullOrEmpty(code))
            {
                string co = WeiXinHelpers.GetUserOpenId(code);

                info = WeiXinHelpers.GetUserInfo(co);
            }
            ViewBag.OpenId = info.OpenId;
            return View();
        }

        /// <summary>
        /// 获取openid
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult DealershipsOpenId(string code)
        {
            if (code != null && code != "")
            {
                string info = WeiXinHelpers.GetUserOpenId(code);

                return Content(info);
            }
            else
            {
                return Content("0");
            }

        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        public ActionResult DealerSgin()
        {
            HttpClient client = new HttpClient();
            var cl = client.GetStringAsync(string.Format("{0}api/Salesman/GetFirms", System.Web.Configuration.WebConfigurationManager.AppSettings["fpsip"])).Result;
            List<Deler> d = JsonHelp.todui<List<Deler>>(cl);
            return View(d);
        }
    }

    public static class JsonHelp
    {
        public static T todui<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}

