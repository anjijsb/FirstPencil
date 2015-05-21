using FirstPencilService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstPencilService.Controllers
{
    public class AuctionController : ApiController
    {
        private static object obj = new object();

        /// <summary>
        /// 返回所有可显示的拍卖
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Auction> GetAuctions()
        {
            var db = new ModelContext();
            return db.AuctionSet.Where(item => item.IsShow).AsEnumerable();
        }

        /// <summary>
        /// 添加拍卖订单
        /// </summary>
        /// <param name="id">拍卖id</param>
        /// <param name="openid">用户openid</param>
        /// <param name="count">拍卖数量</param>
        /// <returns></returns>
        [HttpGet]
        public bool AddOrder(int id, string openid, int count)
        {
            //检测是否已经拍卖
            if (IsAllowAuction(openid, id))
            {
                return false;
            }
            var db = new ModelContext();
            //获取用户信息
            var user = db.UserSet.FirstOrDefault(u => u.OpenId == openid);
            if (user == null)
            {
                return false;
            }
            //获取拍卖信息
            var auction = db.AuctionSet.FirstOrDefault(item => item.AuctionId == id);
            if (auction == null)
            {
                return false;
            }
            //筛选拍卖数量
            if (auction.Count < count || auction.LimitCount < count)
            {
                return false;
            }

            lock (AuctionController.obj)
            {
                //添加拍卖订单
                db.AuctionOrderSet.Add(new AuctionOrder
                {
                    AuctionId = id,
                    UserId = user.UserId,
                    CreatrDate = DateTime.Now,
                    Price = auction.Price.Value * 100 * count / 100,
                    Count = count,
                });
                var rcount = auction.Count - count;
                if (rcount < 0)
                {
                    return false;
                }
                else if (rcount >= 0)
                {
                    auction.Count = rcount;
                }
                //auction.Count -= count;
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 检测是否允许拍卖
        /// </summary>
        /// <param name="openid">用户openid</param>
        /// <returns></returns>
        [HttpGet]
        public bool IsAllowAuction(string openid, int auctionId)
        {
            var db = new ModelContext();
            var user = db.UserSet.Include("Salesman").FirstOrDefault(item => item.OpenId == openid);
            if (user == null || !user.IsSalesman || user.Salesman == null)
            {
                return false;
            }

            return !db.AuctionOrderSet.Any(item => item.UserId == user.UserId && item.AuctionId == auctionId);
        }

        /// <summary>
        /// 获得单个拍卖的详细信息
        /// </summary>
        /// <param name="id">拍卖Id</param>
        /// <returns></returns>
        [HttpGet]
        public Auction GetInfo(int id)
        {
            var db = new ModelContext();
            var auction = db.AuctionSet.FirstOrDefault(item => item.AuctionId == id);
            if (auction != null)
            {
                return auction;
            }
            return null;
        }


        /// <summary>
        /// 获得拍卖订单信息
        /// </summary>
        /// <param name="auctionId">拍卖Id</param>
        /// <param name="lastOrderId">最后获取的订单Id，首次获取填写0</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<AuctionOrder> GetOrder(int auctionId, int lastOrderId)
        {
            var db = new ModelContext();
            var ret = db.AuctionOrderSet.Include("User.Salesman").Where(item => item.AuctionId == auctionId && item.OrderId > lastOrderId);
            return ret;
        }



        /// <summary>
        /// 刷新拍卖开始时间
        /// </summary>
        /// <param name="auctionId">拍卖id</param>
        /// <param name="addSeconds">据开始前的秒数</param>
        [HttpGet]
        public void RefreshAuction(int auctionId, int addSeconds)
        {
            var db = new ModelContext();
            var a = db.AuctionSet.FirstOrDefault(item => item.AuctionId == auctionId);
            if (a != null)
            {
                a.StartDate = DateTime.Now.AddSeconds(addSeconds);
                db.SaveChanges();
            }
        }
    }
}
