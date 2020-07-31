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
    }
}