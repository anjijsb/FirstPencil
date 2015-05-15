﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirstPencilWeb.Helps;
using FirstPencilWeb.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FirstPencilWeb.Controllers
{
    /// <summary>
    /// 功能
    /// </summary>
    public class FunctionController : Controller
    {
        public string ip = System.Web.Configuration.WebConfigurationManager.AppSettings["fpsip"].ToString();
        public static List<Message> messold = new List<Message>();
        public static int maxmsgid;
        public static int num = 0;

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
            HttpClient client = new HttpClient();
            var cl = client.GetStringAsync(string.Format("{0}api/DiscussMsg/GetMsgList?lastId={1}", this.ip, 0)).Result;
            if (cl.Length > 50)
            {
                messold = JsonHelp.todui<List<Message>>(cl);
                maxmsgid = messold.Max(x => x.MsgId);
                List<Message> m = new List<Message>();
                m = messold.Take<Message>(4).ToList();
                messold.RemoveRange(0, m.Count());
                ViewBag.list = m;
            }
            return View();
        }

        /// <summary>
        /// 摇奖
        /// </summary>
        /// <returns></returns>
        public ActionResult Ernie()
        {
            return View();
        }

        /// <summary>
        /// 每两秒刷新留言
        /// </summary>
        /// <returns></returns>
        public JsonResult Add()
        {
            Message m = new Message();
            if (messold.Count() > 0)
            {
                m = messold.FirstOrDefault();
                JsonResult json = Json(new { Headimgurl = m.User.Headimgurl, NickName = m.User.NickName, Content = m.Content, CreateDate = m.CreateDate }, JsonRequestBehavior.AllowGet);
                messold.RemoveRange(0, 1);
                return json;
            }
            else
            {
                HttpClient client = new HttpClient();
                var cl = client.GetStringAsync(string.Format("{0}api/DiscussMsg/GetMsgList?lastId={1}", this.ip, maxmsgid)).Result;
                if (cl.Length < 50)
                {
                    return Json(new { msg = "no" }, JsonRequestBehavior.AllowGet);
                }
                messold = JsonHelp.todui<List<Message>>(cl);
                maxmsgid = messold.Max(x => x.MsgId);
                m = messold.FirstOrDefault();
                JsonResult json = Json(new { Headimgurl = m.User.Headimgurl, NickName = m.User.NickName, Content = m.Content, CreateDate = m.CreateDate }, JsonRequestBehavior.AllowGet);
                messold.RemoveRange(0, 1);
                return json;
            }

        }

    }
}