using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPencilService.Models
{
    public class Auction
    {
        [Key]
        public int AuctionId { get; set; }

        public string Name { get; set; }

        public string PrductName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Double? Price { get; set; }

        public int? Count { get; set; }

        public int? TotalCount { get; set; }

        public int? LimitCount { get; set; }

        public string TitleImg { get; set; }

        public string Descripiton { get; set; }

        public string DescribeImgs { get; set; }

        public string Remarks { get; set; }
    }

    public class AuctionOrder
    {
        [Key]
        public int OrderId { get; set; }

        public int? AuctionId { get; set; }

        [ForeignKey("AuctionId")]
        public virtual Auction Auction { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public double? Price { get; set; }

        public int? Count { get; set; }

        public DateTime? CreatrDate { get; set; }

        public string Remarks { get; set; }
    }


}
