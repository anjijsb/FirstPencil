using FirstPencilService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstPencilService.Controllers
{
    public class QueryCodeController : ApiController
    {
        [HttpGet]
        public void FinishQueryCode(string openId)
        {
            var db = new ModelContext();
            var user = db.UserSet.Include("Salesman").FirstOrDefault(item => item.OpenId == openId);
            if (user == null || !user.IsSalesman || user.Salesman == null)
            {
                return;
            }
            var ap = db.AddPointSet.FirstOrDefault(item => item.Type == AddPointType.QueryCode);
            user.Point = user.Point ?? 0 + ap.Count;
            db.SaveChanges();
        }
    }
}
