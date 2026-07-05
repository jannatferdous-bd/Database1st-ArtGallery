using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ArtGallery.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(): base("AppDbContext")
        {
            
        }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<tblRole> tblRoles { get; set; }
        public virtual DbSet<PayMethod> PayMethods { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SaleDetail> SaleDetails { get; set; }
    }
}