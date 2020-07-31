using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STS.Models
{
    public class Shipment
    {
        [Key]
        public string TrackingNumber { set; get; }
        [StringLength(90)]
        public string SenderName{ set; get; }
        [Phone]
        public string SenderPhoneNumber { set; get; }
        [StringLength(90)]
        public string ReceiverName { set; get; }
        [Phone]
        public string ReceiverPhoneNumber { set; get; }
        public float WeightKG { set; get; }
        [StringLength(255)]
        public string Description { set; get; }
        public DateTime DateAdded { set; get; }
        public byte Status { set; get; }
        public Location CurrentLocation { set; get; }
        public Location Source { set; get; }
        public Location Destination { set; get; }
    }
}