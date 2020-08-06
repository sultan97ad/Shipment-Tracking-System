using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STS.Models
{
    public class Location
    {
        public int Id { set; get; }
        [StringLength(70)]
        public string LocationName { set; get; }
        [StringLength(30)]
        public string City { set; get; }
        public bool CanBeDestination { set; get; }
        public bool InService { set; get; }
        public double Latitude { set; get; }
        public double longitude { set; get; }
    }
}