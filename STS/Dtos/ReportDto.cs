using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STS.Dtos
{
    public class ReportDto
    {
        public string DateTime { set; get; }
        public string Location { set; get; }
        public string Statement { set; get; }
    }
}