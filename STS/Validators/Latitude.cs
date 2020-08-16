using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using STS.Models;
using STS.ViewModels;
using System.Data.Entity;

namespace STS.Validators
{
    public class Latitude : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            double Latitude;
            bool IsDouble = Double.TryParse((string)value, out Latitude);
            if (IsDouble)
            {
                if (Latitude > -90 || Latitude < 90)
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(STS.Resources.Views.Locations.InvalidLatitude);
        }
    }
}
