using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtGallery.ViewModels
{
    public class PastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime date = (DateTime)value;
                if (date > DateTime.Now)
                {
                    return new ValidationResult(ErrorMessage ?? "Date cannot be in the future");
                }
            }
            return ValidationResult.Success;
        }
    }
}