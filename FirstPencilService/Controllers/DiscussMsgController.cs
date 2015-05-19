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

            if (prize.Remarkes != null && prize.Remarkes.StartsWith("#"))
            {
                if (prize.IsActive)
                {
                    prize.IsActive = false;
                    db.SaveChanges();
                    return prize.Remarkes.Replace("#", "");
                }
                else
                {
                    return null;
                }
            }
            else
            {
                var disList = db.DiscussMsgSet.Include("User").Where(item => item.User.IsSalesman).Select<DiscussMsg, int>(item => item.UserId.Value).Distinct().ToList();
                if (disList.Count() != 0)
                {
                    //删除已经获奖人
                    foreach (var item in (prize.Remarkes ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        disList.Remove(int.Parse(item));
                    }
                    if (disList.Count < 1)
                    {
                        return null;
                    }
                    //获取随机值
                    var ran = new Random();
                    var index = ran.Next(disList.Count() - 1);
                    var id = disList.ToArray()[index];
                    //添加被抽中备注
                    prize.Remarkes += (id.ToString() + ",");
                    var user = db.UserSet.Include("Salesman").FirstOrDefault(item => item.UserId == id);
                    db.SaveChanges();
                    return user.Salesman.Name;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 还原抽奖
        /// </summary>
        [HttpGet]
        public void ReSet()
        {
            var db = new ModelContext();
            foreach (var item in db.PrizeSet)
            {
                item.IsActive = true;
                if (!(item.Remarkes ?? "").StartsWith("#"))
                {
                    item.Remarkes = "";
                }
            }
            db.SaveChanges();
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
