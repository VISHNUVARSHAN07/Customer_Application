using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleProjectWebApplication.Models
{
    public class CustomerDetails
    {
        public int Customer_Id { get; set; }
        public string Name { get; set;}
        public DateTime? Valid_From { get; set;}
        public DateTime? Valid_To { get; set;}
        public string Customer_Type { get; set;}
        public string Customer_Status { get; set;}
        public string Phone_Number {  get; set;}   
        
        public List<Personal_Details> Personal_Details { get; set;} = new List<Personal_Details>();
    }
}