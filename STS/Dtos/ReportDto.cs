using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STS.Dtos
{
    public class TrackingRecordDto
    {
        public string DateTime { set; get; }
        public string Location { set; get; }
        public string Statement { set; get; }
    }
}