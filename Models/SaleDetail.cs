using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtGallery.Models
{
    public class SaleDetail
    {
        public int SaleDetailId { get; set; }
        public string ArtName { get; set; }
        public int Quantity { get; set; }
        public int SaleId { get; set; }

        public virtual Sale Sale { get; set; }
    }
}