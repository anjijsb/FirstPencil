using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirstPencilWeb.Helps;

namespace FirstPencilWeb.Controllers
{
    public class FunctionController : Controller
    {
        // GET: Function
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 打假投诉
        /// </summary>
        /// <returns></returns>
        public ActionResult FakeComplaints()
        {
            return View();
        }

        /// <summary>
        /// 防伪查询
        /// </summary>
        /// <returns></returns>
        public ActionResult SecurityCheck()
        {
            return View();
        }

        public ActionResult VerificationCode()
        {
            VerificationCode vCode = new VerificationCode();
            string code = vCode.CreateValidateCode(4);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
    }
}