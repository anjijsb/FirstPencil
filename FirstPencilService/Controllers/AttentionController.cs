using FirstPencilService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstPencilService.Controllers
{
    public class AttentionController : ApiController
    {
        /// <summary>
        /// 获得所有公告列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Attention> GetAttentionList()
        {
            var db = new ModelContext();
            return from att in db.AttentionSet
                   where att.IsActive
                   select att;
        }

        /// <summary>
        /// 获得单个公告信息
        /// </summary>
        /// <param name="attentionId"></param>
        /// <returns></returns>
        [HttpGet]
        public Attention GetAttentionInfo(int attentionId)
        {
            var db = new ModelContext();
            return db.AttentionSet.FirstOrDefault(item => item.AttentionId == attentionId);
        }

        /// <summary>
        /// 获取用户是否可以对该公告进行签到
        /// </summary>
        /// <param name="openId">用户openid</param>
        /// <param name="attentionId">公告id</param>
        /// <returns></returns>
        [HttpGet]
        public bool AllowRegister(string openId, int attentionId)
        {
            var db = new ModelContext();
            //var user = db.UserSet.FirstOrDefault(item => item.OpenId == openId);
            //if (user == null)
            //{
            //    return false;
            //}
            return !db.AttentionRegisterSet.Include("User").Any(item => item.AttentionId == attentionId && item.User.OpenId == openId);
        }

        /// <summary>
        /// 公告签到
        /// </summary>
        /// <param name="openId">用户openid</param>
        /// <param name="attentionId">公告id</param>
        /// <returns></returns>
        [HttpGet]
        public bool Register(string openId, int attentionId)
        {
            if (!AllowRegister(openId, attentionId))
            {
                return false;
            }
            var db = new ModelContext();
            var user = db.UserSet.Include("Salesman").FirstOrDefault(item => item.OpenId == openId);
            if (user == null)
            {
                return false;
            }
            var att = db.AttentionSet.FirstOrDefault(item => item.AttentionId == attentionId);
            if (att == null)
            {
                return false;
            }
            db.AttentionRegisterSet.Add(new AttentionRegister
            {
                UserId = user.UserId,
                CreateDate = DateTime.Now,
                AttentionId = att.AttentionId,
            });
            //添加积分
            if (user.IsSalesman && user.Salesman != null)
            {
                var ap = db.AddPointSet.FirstOrDefault(item => item.Type == AddPointType.AttentionRegister);
                if (ap != null)
                {
                    user.Point += ap.Count;
                }
            }

            return db.SaveChanges() > 0;
        }
    }
}
