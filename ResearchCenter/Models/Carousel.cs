using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResearchCenter.Models
{
    public class Carousel
    {
        public int id { get; set; }
        public string description { get; set; }
        public HttpPostedFileBase image { get; set; }
        public string imageString { get; set; }
    }
}