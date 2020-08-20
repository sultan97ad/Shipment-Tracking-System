using STS.Models;
using STS.Validators;
using System.ComponentModel.DataAnnotations;

namespace STS.ViewModels
{
    public class StatisticsIndexViewModel
    {
        [Display(Name = "RegisteredShipments", ResourceType = typeof(Resources.Views.Statistics))]
        public string RegisteredShipments { set; get; }
        [Display(Name = "ShipmentsWaitingShipping", ResourceType = typeof(Resources.Views.Statistics))]
        public string ShipmentsWaitingShipping { set; get; }
        [Display(Name = "ShipmentsWaitingCollection", ResourceType = typeof(Resources.Views.Statistics))]
        public string ShipmentsWaitingCollection { set; get; }
        [Display(Name = "CollectedShipments", ResourceType = typeof(Resources.Views.Statistics))]
        public string CollectedShipments { set; get; }
        [Display(Name = "NewShipments", ResourceType = typeof(Resources.Views.Statistics))]
        public string NewShipments { set; get; }
        [Display(Name = "ShipmentsCollection", ResourceType = typeof(Resources.Views.Statistics))]
        public string ShipmentsCollection { set; get; }
    }
}