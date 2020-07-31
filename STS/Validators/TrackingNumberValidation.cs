using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using STS.Models;
using STS.ViewModels;

namespace STS.Validators
{
    public class TrackingNumberValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ViewModel = ((MainIndexViewModel)validationContext.ObjectInstance);
            var Shipment = new ApplicationDbContext().Shipments.SingleOrDefault(c => c.TrackingNumber == ViewModel.TrackingNumber);
            if (Shipment != null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(STS.Resources.Views.Main.InvalidTrackingNumber);
        }
    }
}
