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
            HttpClient client = new HttpClient();
            var cl = client.GetStringAsync(string.Format("{0}api/User/GetUserInfoByOpenid?openid={1}", this.ip, "o-ZC8sxsIpHFrOORZjNmVL_u29oI")).Result;
            FirstPencilService.Models.User user = JsonHelp.todui<FirstPencilService.Models.User>(cl);

            var cl1 = client.GetStringAsync(string.Format("{0}api/Salesman/Level?point={1}", this.ip, user.Point)).Result;
            cl1 = cl1.Replace("\"", "");
            string[] points = cl1.Split(';');
            if (points[1] != "")
            {
                ViewBag.num = "/" + points[1];
                ViewBag.dj = ((float)user.Point / float.Parse(points[1])) * 100;
            }
            else
            {
                ViewBag.num = "";
                ViewBag.dj = 100;
            }
            switch (points[0])
            {
                case "普通": ViewBag.url = "http://www.anjismart.com/FirstPencilWeb/Images/dj/pt.png"; break;
                case "白银": ViewBag.url = "http://www.anjismart.com/FirstPencilWeb/Images/dj/by.png"; break;
                case "黄金": ViewBag.url = "http://www.anjismart.com/FirstPencilWeb/Images/dj/hj.png"; break;
                case "钻石": ViewBag.url = "http://www.anjismart.com/FirstPencilWeb/Images/dj/zs.png"; break;
                case "皇冠": ViewBag.url = "http://www.anjismart.com/FirstPencilWeb/Images/dj/hg.png"; break;
            }
            ViewBag.djname = points[0];
            ViewBag.username = user.Salesman.Name;
            ViewBag.headimgurl = user.Headimgurl;
            ViewBag.poits = user.Point;
            var cl2 = client.GetStringAsync(string.Format("{0}api/Salesman/GetSalesmanPointOrder?takeNumber={1}", this.ip, 5)).Result;
            if (cl2.Length > 50)
            {
                List<FirstPencilService.Models.User> users = JsonHelp.todui<List<FirstPencilService.Models.User>>(cl2);
                ViewBag.userslist = users;
            }
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

