using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstPencilWeb.Models
{
    public class Auctions
    {
        public int AuctionId { get; set; }
        public string Name { get; set; }
        public string PrductName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public float Price { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
        public int LimitCount { get; set; }
        public string TitleImg { get; set; }
        public Description Descripiton { get; set; }
        public string DescribeImgs { get; set; }
        public bool IsShow { get; set; }
        public string Remarks { get; set; }

    }
    public class Description
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string Spec { get; set; }
        public string Hardness { get; set; }
    }
}