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
    public class Longitude : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            double Longitude;
            bool IsDouble = Double.TryParse((string)value, out Longitude);
            if (IsDouble)
            {
                if (Longitude > -180 || Longitude < 180)
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(STS.Resources.Views.Locations.InvalidLongitude);
        }
    }
}

