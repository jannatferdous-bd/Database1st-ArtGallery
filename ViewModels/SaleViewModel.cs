using ArtGallery.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtGallery.ViewModels
{
    public class SaleViewModel
    {
        public int SaleId { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Customer name must be between 3 and 50 characters.")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        
        [Required(ErrorMessage = "Sale Date  is required.")]
        [Display(Name = "Sale Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [PastDate(ErrorMessage = "Sale date cannot be a future date")]
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Phone Number is required.")]

        [Display(Name = "Customer Phone ")]
        public string CustomerPhone { get; set; }

        [Display(Name = "Paid?")]
        public bool IsPaid { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Total price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total price must be greater than zero")]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "Please select a payment method")]
        [Display(Name = "Payment Method")]
        public int PayMethodId { get; set; }

        [Display(Name = "PayMethod Name")]
        public string PayMethodName { get; set; }

        public int SaleDetailId { get; set; }
        [Display(Name = "Art Name")]
        public string ArtName { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Upload Picture")]
        public HttpPostedFileBase ProfileFile { get; set; }




        public virtual IList<PayMethod> PayMethods { get; set; }
        public virtual IList<Sale> Sales { get; set; }
        public virtual IList<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();

    }
}