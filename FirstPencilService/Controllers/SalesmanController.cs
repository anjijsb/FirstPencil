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
            var ret = WechatHelper.GetCodeForMeeting(sm);
            return ret.CodeUrl;
        }


    }
}
