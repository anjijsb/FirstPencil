using FirstPencilService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstPencilService.Controllers
{
    public class SalesmanController : ApiController
    {
        /// <summary>
        /// 获取厂商列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Firm> GetFirms()
        {
            var db = new ModelContext();
            return db.FirmSet.Where(item => 1 == 1).AsEnumerable();
        }

        /// <summary>
        /// 添加经销商人员并获取其绑定二维码
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="Position"></param>
        /// <returns></returns>
        [HttpGet]
        public string AddSalesman(int firmId, string name, string phone, string Position)
        {
            var db = new ModelContext();
            var firm = db.FirmSet.FirstOrDefault(item => item.FirmId == firmId);
            if (firm == null)
            {
                return null;
            }
            var sm = new Salesman
            {
                FirmId = firmId,
                Name = name,
                Position = Position,
                PhoneNumber = phone,
            };
            db.SalesmanSet.Add(sm);
            db.SaveChanges();
            var ret = WechatHelper.GetCodeForMeeting();
            ret.Remarks = sm.SalesmanId.ToString();
            db.ScanEventSet.Add(ret);
            db.SaveChanges();
            return ret.CodeUrl;
        }



        /// <summary>
        /// 检测二维码是否有效   
        /// </summary>
        /// <param name="url">二维码链地址</param>
        /// <returns></returns>
        [HttpGet]
        public bool IsActive(string url)
        {
            var db = new ModelContext();
            var code = db.ScanEventSet.FirstOrDefault(item => item.CodeUrl == url);
            if (code != null)
            {
                return code.IsActive.Value;
            }
            else
            {
                return false;
            }
        }

    }

    ///// <summary>
    ///// 检测用户否已经扫码注册
    ///// </summary>
    //public class SalesmanAsynController : System.Web.Mvc.AsyncController
    //{

    //    [HttpGet]
    //    public void LongPollingAsync(string openid)
    //    {
    //        //计时器，5秒种触发一次Elapsed事件
    //        System.Timers.Timer timer = new System.Timers.Timer(500);
    //        //告诉ASP.NET接下来将进行一个异步操作
    //        AsyncManager.OutstandingOperations.Increment();
    //        //订阅计时器的Elapsed事件
    //        timer.Elapsed += (sender, e) =>
    //        {
    //            var db = new ModelContext();
    //            var user = db.UserSet.FirstOrDefault(item => item.OpenId == openid);
    //            if (user != null && user.IsSalesman)
    //            {
    //                AsyncManager.OutstandingOperations.Decrement();
    //            }
    //        };
    //        //启动计时器
    //        timer.Start();
    //    }

    //    //LongPolling Action 2 - 异步处理完成，向客户端发送响应
    //    public bool LongPollingCompleted()
    //    {
    //        return true;
    //    }
    //}
}
