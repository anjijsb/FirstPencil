using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPencilService.Models
{
    class ModelContext : DbContext
    {
        public ModelContext() : base("ModelContextConnection") { }

        public DbSet<User> UserSet { get; set; }

        public DbSet<Complain> ComplainSet { get; set; }

        public DbSet<Firm> FirmSet { get; set; }

        public DbSet<Salesman> SalesmanSet { get; set; }

        public DbSet<ScanEvent> ScanEventSet { get; set; }

        public DbSet<AccessToken> AccessTokenSet { get; set; }

        public DbSet<DiscussMsg> DiscussMsgSet { get; set; }

        public DbSet<Prize> PrizeSet { get; set; }

        public DbSet<Auction> AuctionSet { get; set; }

        public DbSet<AuctionOrder> AuctionOrderSet { get; set; }

        public DbSet<AddPoint> AddPointSet { get; set; }
    }
}
