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

        private string ip = System.Web.Configuration.WebConfigurationManager.AppSettings["fpsip"].ToString();
        private static List<FirstPencilService.Models.AuctionOrder> messold = new List<FirstPencilService.Models.AuctionOrder>();
        private List<FirstPencilService.Models.AuctionOrder> mold = new List<FirstPencilService.Models.AuctionOrder>();
        private static int maxorderid;

        private static string openid;
        private static int num = 9;
        private string aid = System.Web.Configuration.WebConfigurationManager.AppSettings["aid"].ToString();
        private int mainnum = 0;
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

        /// <summary>
        /// 拍卖倒计时
        /// </summary>
        /// <returns></returns>
        public ActionResult AuctionShow()
        {
            HttpClient client = new HttpClient();
            var cl1 = client.GetStringAsync(string.Format("{0}api/Auction/RefreshAuction?auctionId={1}&addSeconds={2}", this.ip, this.aid, 60)).Result;
            var cl = client.GetStringAsync(string.Format("{0}api/Auction/GetInfo/{1}", this.ip, this.aid)).Result;
            cl = cl.Replace("\"{", "{");
            cl = cl.Replace("}\"", "}");
            cl = cl.Replace("\\", "");
            Auctions d = JsonHelp.todui<Auctions>(cl);
            ViewBag.AuctionsList = d;
            return View();
        }

        /// <summary>
        /// 拍卖详情数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AuctionShowd()
        {
            HttpClient client = new HttpClient();
            #region 购买信息
            var cl = client.GetStringAsync(string.Format("{0}api/Auction/GetOrder?auctionId={1}&lastOrderId={2}", this.ip, this.aid, 0)).Result;
            if (cl.Length > 50)
            {
                messold = JsonHelp.todui<List<FirstPencilService.Models.AuctionOrder>>(cl);
                mold = JsonHelp.todui<List<FirstPencilService.Models.AuctionOrder>>(cl);
                if (mold.Count() > num)
                {
                    mold.RemoveAt(0);
                }
                maxorderid = messold.Max(x => x.OrderId);
                List<FirstPencilService.Models.AuctionOrder> m = new List<FirstPencilService.Models.AuctionOrder>();
                m = messold.Take<FirstPencilService.Models.AuctionOrder>(num).ToList();
                messold.RemoveRange(0, m.Count());
                ViewBag.AuctionOrderList = m;
            }
            #endregion
            #region 数量
            var cl1 = client.GetStringAsync(string.Format("{0}api/Auction/GetInfo/{1}", this.ip, this.aid)).Result;
            if (cl1.Length > 50)
            {
                cl1 = cl1.Replace("\"{", "{");
                cl1 = cl1.Replace("}\"", "}");
                cl1 = cl1.Replace("\\", "");
                Auctions e = JsonHelp.todui<Auctions>(cl1);
                ViewBag.Auctions = e;
            }
            #endregion
            return View();
        }

        /// <summary>
        /// 拍卖详情购买信息滚动添加
        /// </summary>
        /// <returns></returns>
        public JsonResult AuctionShowAdd()
        {
            FirstPencilService.Models.AuctionOrder a = new FirstPencilService.Models.AuctionOrder();
            if (messold.Count() > 0)
            {
                a = messold.FirstOrDefault();
                JsonResult json = Json(new { Name = a.User.Salesman.Name, CreatrDate = a.CreatrDate.ToString(), Count = a.Count }, JsonRequestBehavior.AllowGet);
                messold.RemoveAt(0);
                return json;
            }
            else
            {
                HttpClient client = new HttpClient();
                var cl = client.GetStringAsync(string.Format("{0}api/Auction/GetOrder?auctionId={1}&lastOrderId={2}", this.ip, this.aid, maxorderid)).Result;
                if (cl.Length < 50)
                {
                    return Json(new { msg = "no" }, JsonRequestBehavior.AllowGet);
                }
                messold = JsonHelp.todui<List<FirstPencilService.Models.AuctionOrder>>(cl);
                maxorderid = messold.Max(x => x.OrderId);
                a = messold.FirstOrDefault();
                JsonResult json = Json(new { Name = a.User.Salesman.Name, CreatrDate = a.CreatrDate.ToString(), Count = a.Count }, JsonRequestBehavior.AllowGet);
                messold.RemoveAt(0);
                return json;
            }

        }

        /// <summary>
        /// 刷新数量
        /// </summary>
        /// <returns></returns>
        public JsonResult AuctionShowNum()
        {
            HttpClient client = new HttpClient();
            var cl1 = client.GetStringAsync(string.Format("{0}api/Auction/GetInfo/{1}", this.ip, this.aid)).Result;
            cl1 = cl1.Replace("\"{", "{");
            cl1 = cl1.Replace("}\"", "}");
            cl1 = cl1.Replace("\\", "");
            Auctions e = JsonHelp.todui<Auctions>(cl1);
            float bai = ((float)e.Count / (float)e.TotalCount) * 100;
            return Json(new { Count = e.Count, TotalCount = e.TotalCount, bai = bai }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 拍卖事件
        /// </summary>
        /// <param name="id">拍卖商品ID</param>
        /// <param name="oid">openid</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
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