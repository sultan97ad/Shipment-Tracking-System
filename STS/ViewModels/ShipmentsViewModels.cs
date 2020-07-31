using STS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace STS.ViewModels
{
    public class ShipmentFormViewModel
    {
        public IEnumerable<SelectListItem> Locations { get; set; }
        public string TrackingNumber { set; get; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Shipments))]
        [StringLength(90, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(Resources.Views.Shipments))]
        [Display(Name = "SenderName", ResourceType = typeof(Resources.Views.Shipments))]
        public string SenderName { set; get; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Shipments))]
        [Phone(ErrorMessageResourceName = "IsPhone", ErrorMessageResourceType = typeof(Resources.Views.Shipments))]
        [Display(Name = "SenderPhoneNumber", ResourceType = typeof(Resources.Views.Shipments))]
        public string SenderPhoneNumber { set; get; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Shipments))]
        [StringLength(90, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(Resources.Views.Shipments))]
        [Display(Name = "ReceiverName", ResourceType = typeof(Resources.Views.Shipments))]
        public string ReceiverName { set; get; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Shipments))]
        [Phone(ErrorMessageResourceName = "IsPhone", ErrorMessageResourceType = typeof(Resources.Views.Shipments))]
        [Display(Name = "ReceiverPhoneNumber", ResourceType = typeof(Resources.Views.Shipments))]
        public string ReceiverPhoneNumber { set; get; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Shipments))]
        [Display(Name = "WeightKG", ResourceType = typeof(Resources.Views.Shipments))]
        public float WeightKG { set; get; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Shipments))]
        [StringLength(255, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(Resources.Views.Shipments))]
        [Display(Name = "Description", ResourceType = typeof(Resources.Views.Shipments))]
        public string Description { set; get; }
        [Required(ErrorMessageResourceName = "DestinationRequired", ErrorMessageResourceType = typeof(Resources.Views.Shipments))]
        [Display(Name = "Destination", ResourceType = typeof(Resources.Views.Shipments))]
        public int DestinationLocationId { get; set; }
    }

    public class ShipmentDetailsViewModel
    {
        [Display(Name = "TrackingNumber", ResourceType = typeof(Resources.Views.Shipments))]
        public string TrackingNumber { set; get; }
        [Display(Name = "SenderName", ResourceType = typeof(Resources.Views.Shipments))]
        public string SenderName { set; get; }
        [Display(Name = "SenderPhoneNumber", ResourceType = typeof(Resources.Views.Shipments))]
        public string SenderPhoneNumber { set; get; }
        [Display(Name = "ReceiverName", ResourceType = typeof(Resources.Views.Shipments))]
        public string ReceiverName { set; get; }
        [Display(Name = "ReceiverPhoneNumber", ResourceType = typeof(Resources.Views.Shipments))]
        public string ReceiverPhoneNumber { set; get; }
        [Display(Name = "WeightKG", ResourceType = typeof(Resources.Views.Shipments))]
        public float WeightKG { set; get; }
        [Display(Name = "Description", ResourceType = typeof(Resources.Views.Shipments))]
        public string Description { set; get; }
        [Display(Name = "DateAdded", ResourceType = typeof(Resources.Views.Shipments))]
        public string DateAdded { set; get; }
        [Display(Name = "Source", ResourceType = typeof(Resources.Views.Shipments))]
        public string Source { set; get; }
        [Display(Name = "Destination", ResourceType = typeof(Resources.Views.Shipments))]
        public string Destination { set; get; }
    }
}