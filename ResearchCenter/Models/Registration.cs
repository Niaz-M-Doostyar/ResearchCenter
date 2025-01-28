using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace ResearchCenter.Models
{
    public class Registration
    {
        public int id { get; set; }
        public string name { get; set; }
        public string designation { get; set; }
        public string department { get; set; }
        public string faculty { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        [DataType(DataType.Password)]
        public string password { get; set; }
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string confirmPassword { get; set; }
        public string status { get; set; }
    }
}