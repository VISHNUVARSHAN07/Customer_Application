﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleProjectWebApplication.Models
{
    public class UserRegisteration
    {
        public int User_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Dob { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
    }
}