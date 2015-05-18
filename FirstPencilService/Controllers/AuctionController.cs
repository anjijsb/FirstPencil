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
            var db = new ModelContext();
            var user = db.UserSet.FirstOrDefault(u => u.OpenId == openid);
            if (user == null)
            {
                return false;
            }
            var auction = db.AuctionSet.FirstOrDefault(item => item.AuctionId == id);
            if (auction == null)
            {
                return false;
            }
            if (auction.Count < count || auction.LimitCount < count)
            {
                return false;
            }
            db.AuctionOrderSet.Add(new AuctionOrder
            {
                AuctionId = id,
                UserId = user.UserId,
                CreatrDate = DateTime.Now,
                Price = auction.Price * count,
                Count = count,
            });

            auction.Count -= count;
            return db.SaveChanges() == 1;
        }

        [HttpGet]
        public Auction GetInfo(int id)
        {
            var db = new ModelContext();
            var auction = db.AuctionSet.FirstOrDefault(item => item.AuctionId == id);
            if(auction != null)
            {
                return auction;
            }
            return null;
        }

    }
}
