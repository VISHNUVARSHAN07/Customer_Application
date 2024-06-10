using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SampleProjectWebApplication.Models
{
    public class Login
    {
        [Display(Name = "Enter User Name")]
        public string User_Name { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Enter Password")]
        public string Password { get; set; }
    }
}