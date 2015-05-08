using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstPencilWeb.Controllers
{
    /// <summary>
    /// 企业
    /// </summary>
    public class EnterpriseController : Controller
    {
        /// <summary>
        /// 企业介绍
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

    }
}