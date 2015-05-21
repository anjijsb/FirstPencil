using FirstPencilWeb.Helps;
using FirstPencilWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FirstPencilWeb.Controllers
{
    public class UsersController : Controller
    {
        public string ip = System.Web.Configuration.WebConfigurationManager.AppSettings["fpsip"].ToString();
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
            var cl = client.GetStringAsync(string.Format("{0}api/Salesman/GetFirms", this.ip)).Result;
            List<Deler> d = JsonHelp.todui<List<Deler>>(cl);
            ViewBag.list = d;
            ViewBag.ip = this.ip;
            return View();
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public JsonResult TiJiao(string firmId, string name, string phone, string position)
        {
            HttpClient client = new HttpClient();
            var cl = client.GetStringAsync(string.Format("{0}api/Salesman/AddSalesman?firmId={1}&name={2}&phone={3}&Position={4}", this.ip, firmId, name, phone, position)).Result;
            return Json(new { url = cl.ToString() }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 判断二维码是否生效
        /// </summary>
        /// <returns></returns>
        public JsonResult erweimaxiaodiao(string url)
        {
            HttpClient client = new HttpClient();
            var cl = client.GetStringAsync(string.Format("{0}api/Salesman/IsActive?url={1}", this.ip, url)).Result;
            return Json(new { b = cl.ToString() }, JsonRequestBehavior.AllowGet);
        }

    }


    #region Json帮助

    public static class JsonHelp
    {
        public static T todui<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }


    #endregion
}

