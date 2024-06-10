using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleProjectWebApplication.Models
{
    public class Personal_Details
    {

        public int Customer_Id { get; set; }
        public int Personal_Id { get; set; }
        public string Address_Type { get; set;}
        public string Street  { get; set;}
        public string Area { get; set;}
        public string Location { get; set;}
        public string Pincode { get; set;}
        public bool IsDeleted { get; set; }

        public int RowIndex { get; set; }
    }
}