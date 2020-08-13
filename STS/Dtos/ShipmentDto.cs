using STS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STS.Dtos
{
    public class ShipmentDto
    {
        public string TrackingNumber { set; get; }
        public string ReceiverName { set; get; }
        public string Destination { set; get; }
        public string Status { set; get; }
        public string HoldSince { set; get; }
    }
}