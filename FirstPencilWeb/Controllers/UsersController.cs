using FirstPencilWeb.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

    }
}