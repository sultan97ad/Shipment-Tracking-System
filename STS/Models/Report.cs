using System;
using System.ComponentModel.DataAnnotations;

namespace STS.Models
{
    public class Report
    {
        public int Id { set; get; }
        public Shipment Shipment { set; get; }
        public Location Location { set; get; }
        public byte Event { set; get; }
        public DateTime DateTime { set; get; }
        public string SignedBy { set; get; }
    }
}