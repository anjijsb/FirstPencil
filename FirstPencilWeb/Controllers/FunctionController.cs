using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirstPencilWeb.Helps;
using FirstPencilWeb.Models;

namespace FirstPencilWeb.Controllers
{
    /// <summary>
    /// 功能
    /// </summary>
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

        /// <summary>
        /// 图片防伪码
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        public ActionResult VerificationCode(string rid)
        {
            VerificationCode vCode = new VerificationCode();
            string code = vCode.CreateValidateCode(4);
            Session["ValidateCode"] = code;
            var code1 = SessionKey.ValidateCode;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }


        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckCode()
        {
            string quickChannalConfirmCode = Request.QueryString["code"].ToString();
            if (Session["ValidateCode"] == null || "".Equals(Session["ValidateCode"].ToString()))
            {
                return Content("false");
            }
            string yanzheng = Session["ValidateCode"].ToString();
            if (quickChannalConfirmCode == yanzheng)
            {
                Session.Remove("ValidateCode");
                return Content("true");
            }
            else
            {
                return Content("false");
            }
        }


        /// <summary>
        /// 留言墙
        /// </summary>
        /// <returns></returns>
        public ActionResult Messagewall()
        {
            return View();
        }
    }
}