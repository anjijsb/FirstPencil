using FirstPencilWeb.Helps;
using FirstPencilWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace FirstPencilWeb.Controllers
{
    public class BusinessController : Controller
    {

        public string ip = System.Web.Configuration.WebConfigurationManager.AppSettings["fpsip"].ToString();
        public static string openid;
        // GET: Business
        public ActionResult Index()
        {
            return View();
        }

        #region 拍卖
        /// <summary>
        /// 拍卖详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult Auction(string id)
        {
            HttpClient client = new HttpClient();
            var cl = client.GetStringAsync(string.Format("{0}api/Auction/GetInfo/{1}", this.ip, id)).Result;
            cl = cl.Replace("\"{", "{");
            cl = cl.Replace("}\"", "}");
            cl = cl.Replace("\\", "");
            Auctions a = JsonHelp.todui<Auctions>(cl);
            ViewBag.Auctions = a;
            ViewBag.OpenId = openid;
            return View();
        }

        /// <summary>
        /// 拍卖推荐
        /// </summary>
        /// <param name="code">code</param>
        /// https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx13b3ff8fdcc0d04f&redirect_uri=http://www.anjismart.com/first/FirstPencilWeb&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect
        /// <returns></returns>
        public ActionResult AuctionRecommend(string code)
        {
            WeiXinUserInfo info = new WeiXinUserInfo();
            if (!string.IsNullOrEmpty(code))
            {
                string co = WeiXinHelpers.GetUserOpenId(code);
                info = WeiXinHelpers.GetUserInfo(co);
                openid = info.OpenId;
            }

            HttpClient client = new HttpClient();
            var cl = client.GetStringAsync(string.Format("{0}api/Auction/GetAuctions", this.ip)).Result;
            cl = cl.Replace("\"{", "{");
            cl = cl.Replace("}\"", "}");
            cl = cl.Replace("\\", "");
            List<Auctions> d = JsonHelp.todui<List<Auctions>>(cl);
            ViewBag.AuctionsList = d;
            return View();
        }

        public JsonResult AuctionBuy(string id, string oid, string count)
        {
            HttpClient client = new HttpClient();
            var cl = client.GetStringAsync(string.Format("{0}api/Auction/AddOrder/{1}?openid={2}&count={3}", this.ip, id, oid, count)).Result;
            return Json(new { msg = cl }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region 新品

        /// <summary>
        /// 新品推荐
        /// </summary>
        /// <returns></returns>
        public ActionResult NewProductRecommend()
        {
            return View();
        }

        /// <summary>
        /// 新品详情
        /// </summary>
        /// <returns></returns>
        public ActionResult NewProductDetails()
        {
            return View();
        }

        #endregion
    }
}