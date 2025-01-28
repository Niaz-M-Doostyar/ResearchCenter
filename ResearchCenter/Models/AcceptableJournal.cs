using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResearchCenter.Models
{
    public class AcceptableJournal
    {
        public int id { get; set; }
        public HttpPostedFileBase logo { get; set; }
        public string logoString { get; set; }
    }
}