using STS.Models;
using STS.Validators;
using System.ComponentModel.DataAnnotations;

namespace STS.ViewModels
{
    public class MainIndexViewModel
    {
        [TrackingNumberValidation]
        [Display(Name = "TrackingNumber", ResourceType = typeof(Resources.Views.Main))]
        public string TrackingNumber { set; get; }
    }

    
    public class SetDeliveryLocationViewModel
    {
        [Display(Name = "TrackingNumber", ResourceType = typeof(Resources.Views.Main))]
        public string TrackingNumber { set; get; }
        [Display(Name = "ReceiverPhoneNumber", ResourceType = typeof(Resources.Views.Main))]
        public string ReceiverPhoneNumber { set; get; }
        [Latitude]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Main))]
        [Display(Name = "Latitude", ResourceType = typeof(Resources.Views.Main))]
        public string Latitude { set; get; }
        [Longitude]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Main))]
        [Display(Name = "longitude", ResourceType = typeof(Resources.Views.Main))]
        public string longitude { set; get; }
        [DeliveryRangeValidation]
        public string DeliveryRange { set; get; }
    }

    public class TrackShipmentViewModel
    {
        [Display(Name = "TrackingNumber", ResourceType = typeof(Resources.Views.Main))]
        public string TrackingNumber { set; get; }
        [Display(Name = "Status", ResourceType = typeof(Resources.Views.Main))]
        public string Status { set; get; }
        [Display(Name = "CurrentLocation", ResourceType = typeof(Resources.Views.Main))]
        public string CurrentLocation { set; get; }
        [Display(Name = "Source", ResourceType = typeof(Resources.Views.Main))]
        public string Source { set; get; }
        [Display(Name = "Destination", ResourceType = typeof(Resources.Views.Main))]
        public string Destination { set; get; }
        [Display(Name = "DistanceToDestination", ResourceType = typeof(Resources.Views.Main))]
        public string DistanceToDestination { set; get; }
        [Display(Name = "EstimatedDeliveryDate", ResourceType = typeof(Resources.Views.Main))]
        public string EstimatedDeliveryDate { set; get; }
    }
}