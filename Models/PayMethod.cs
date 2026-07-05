using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtGallery.Models
{
    public class PayMethod
    {
        public PayMethod()
        {
            this.Sales = new HashSet<Sale>();
        }
        public int PayMethodId { get; set; }
        public string PayMethodName { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }

    }
}