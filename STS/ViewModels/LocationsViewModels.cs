using System.ComponentModel.DataAnnotations;

namespace STS.ViewModels
{
    public class LocationFormViewModel
    {
        public int LocationId { set; get; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Locations))]
        [StringLength(70 , ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(Resources.Views.Locations))]
        [Display(Name = "LocationName", ResourceType = typeof(Resources.Views.Locations))]
        public string LocationName { set; get; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Locations))]
        [StringLength(30, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(Resources.Views.Locations))]
        [Display(Name = "City", ResourceType = typeof(Resources.Views.Locations))]
        public string City { set; get; }
        [Display(Name = "Latitude", ResourceType = typeof(Resources.Views.Locations))]
        public string Latitude { set; get; }
        [Display(Name = "longitude", ResourceType = typeof(Resources.Views.Locations))]
        public string longitude { set; get; }
        [Display(Name = "CanBeDestination", ResourceType = typeof(Resources.Views.Locations))]
        public bool CanBeDestination { set; get; }
    }

    public class LocationDetailsViewModel
    {
        [Display(Name = "LocationId", ResourceType = typeof(Resources.Views.Locations))]
        public int LocationId { set; get; }
        [Display(Name = "LocationName", ResourceType = typeof(Resources.Views.Locations))]
        public string LocationName { set; get; }
        [Display(Name = "City", ResourceType = typeof(Resources.Views.Locations))]
        public string City { set; get; }
        [Display(Name = "Latitude", ResourceType = typeof(Resources.Views.Locations))]
        public string Latitude { set; get; }
        [Display(Name = "longitude", ResourceType = typeof(Resources.Views.Locations))]
        public string longitude { set; get; }
        [Display(Name = "CanBeDestination", ResourceType = typeof(Resources.Views.Locations))]
        public bool CanBeDestination { set; get; }
        [Display(Name = "NumberOfShipments", ResourceType = typeof(Resources.Views.Locations))]
        public string NumberOfShipments { set; get; }
        [Display(Name = "NumberOfEmployees", ResourceType = typeof(Resources.Views.Locations))]
        public string NumberOfEmployees { set; get; }
    }






}