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
        public ActionResult Dealerships()
        {
            return View();
        }

        public ActionResult DealershipsOpenId()
        {
            if (Request.QueryString["code"].ToString() != null && Request.QueryString["code"].ToString() != "")
            {
                string DealerShipsCode = Request.QueryString["code"].ToString();

                string info = WeiXinHelpers.GetUserOpenId(DealerShipsCode);

                return Content("22");
            }
            else
            {
                return Content("0");
            }

        }

    }
}