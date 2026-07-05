namespace ArtGallery.Migrations
{
    using ArtGallery.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ArtGallery.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ArtGallery.Models.AppDbContext context)
        {
            //var payMethods = new List<PayMethod>
            //{
            //    new PayMethod {PayMethodName="Bkash" },
            //    new PayMethod {PayMethodName="Cash" },
            //    new PayMethod {PayMethodName="Roket" },
            //    new PayMethod {PayMethodName="Check" }
            //};
            //payMethods.ForEach(s => context.PayMethods.AddOrUpdate(p => p.PayMethodName, s));
            //context.SaveChanges();
        }
    }
}
