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
    public class DeliveryRangeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ViewModel = ((SetDeliveryLocationViewModel)validationContext.ObjectInstance);
            var ShipmentLocation = new ApplicationDbContext().Shipments.Include(Shipment => Shipment.CurrentLocation).SingleOrDefault(Shipment => Shipment.TrackingNumber == ViewModel.TrackingNumber).CurrentLocation;
            var GivenLocation = new Location { Latitude = Double.Parse(ViewModel.Latitude), longitude = Double.Parse(ViewModel.longitude) };
            var Distance = CalculateDistance(ShipmentLocation, GivenLocation);
            if(Distance > ShipmentLocation.DeliveryRange)
            {
                return new ValidationResult(STS.Resources.Views.Main.OutOfDeliveryRange);
            }
            return ValidationResult.Success;
        }

        public int CalculateDistance(Location L1, Location L2)
        {
            double lat1 = L1.Latitude;
            double lon1 = L1.longitude;
            double lat2 = L2.Latitude;
            double lon2 = L2.longitude;
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            return Convert.ToInt32(dist * 1.609344);
        }
    }
}
