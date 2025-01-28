using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResearchCenter.Models
{
    public class InternationalPaper
    {
        public int id { get; set; }
        public string familyName { get; set; }
        public string name { get; set; }
        public string authors { get; set; }
        public string department { get; set; }
        public string faculty { get; set; }
        public string designation { get; set; }
        public string speciality { get; set; }
        public string title { get; set; }
        public string artical { get; set; }
        public string journal { get; set; }
        public string journalName { get; set; }
        public string language { get; set; }
        public DateTime year { get; set; }
        public int issue { get; set; }
        public int page { get; set; }
        public string doi { get; set; }
        public HttpPostedFileBase pdf { get; set; }
        public string pdfFile { get; set; }
        public string comment { get; set; }
        public string status { get; set; }

        public int registerId { get; set; }
    }
}