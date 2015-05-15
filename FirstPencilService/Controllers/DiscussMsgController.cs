using FirstPencilService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstPencilService.Controllers
{
    /// <summary>
    /// 大会现场用户互动接口
    /// </summary>
    public class DiscussMsgController : ApiController
    {
        /// <summary>
        /// 获得留言列表
        /// </summary>
        /// <param name="lastId">上次请求最大的id,若为初次请求则填写0</param>
        /// <returns>留言列表（包含用户信息）</returns>
        [HttpGet]
        public IEnumerable<DiscussMsg> GetMsgList(int lastId)
        {
            var db = new ModelContext();
            var ret = db.DiscussMsgSet.Include("User").Where(item => item.MsgId > lastId).AsEnumerable();
            return ret;
        }

        /// <summary>
        /// 获取现有奖项列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Prize> GetPrizeList()
        {
            var db = new ModelContext();
            return db.PrizeSet.Where(item => 1 == 1).AsEnumerable();
        }

        /// <summary>
        /// 获取中奖人名称
        /// </summary>
        /// <param name="prizeId">奖项id</param>
        /// <returns></returns>
        [HttpGet]
        public string GetWinner(int prizeId)
        {
            var db = new ModelContext();
            var prize = db.PrizeSet.FirstOrDefault(item => item.PrizeId == prizeId);
            if (prize == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(prize.Remarkes))
            {
                var ran = new Random();
                var disList = db.DiscussMsgSet.Select<DiscussMsg, int>(item => item.MsgId);
                if (disList.Count() != 0)
                {
                    var index = ran.Next(disList.Count() - 1);
                    var id = disList.ToArray()[index];
                    return db.DiscussMsgSet.Include("User").FirstOrDefault(item => item.MsgId == id).User.NickName;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return prize.Remarkes;
            }
        }

        /// <summary>
        /// 获得候选人名单，用于滚动
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> GetUserNames()
        {
            var db = new ModelContext();
            var nameArr = db.DiscussMsgSet.Include("User").Select(item => item.User.NickName).Distinct().ToList();

            var indexList = new List<int>();
            var retList = new List<string>();

            var ran = new Random();
            if (nameArr.Count() < 20)
            {
                return nameArr;
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    var index = ran.Next(nameArr.Count() - 1);
                    string name = nameArr[index];
                    retList.Add(name);
                    nameArr.Remove(name);
                }
                return retList;
            }
        }

    }
}
