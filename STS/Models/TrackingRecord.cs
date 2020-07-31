using System;
using System.ComponentModel.DataAnnotations;

namespace STS.Models
{
    public class TrackingRecord
    {
        public int Id { set; get; }
        public Shipment Shipment { set; get; }
        public Location Location { set; get; }
        public byte Type { set; get; }
        public DateTime DateTime { set; get; }
        public string SignedBy { set; get; }
    }
}