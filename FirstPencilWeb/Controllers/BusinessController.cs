﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstPencilWeb.Controllers
{
    public class BusinessController : Controller
    {
        // GET: Business
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 拍卖详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult Auction()
        {
            return View();
        }

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
    }
}