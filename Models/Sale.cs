using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtGallery.Models
{
    public class Sale
    {
        public Sale()
        {
            this.SaleDetails = new HashSet<SaleDetail>();
        }
        public int SaleId { get; set; }


        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }


        [Display(Name = "Customer Phone")]
        public string CustomerPhone { get; set; }


        [Display(Name = "IsPaid")]
        public bool IsPaid { get; set; }


        [Required, Display(Name = "Sale Date"), DataType(DataType.Date),
           DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime SaleDate { get; set; } = DateTime.Now;


        [Display(Name = "TotalPrice")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "PayMethod")]     
        public int PayMethodId { get; set; }
        public virtual PayMethod PayMethod { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }



    }
}