using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SampleProjectWebApplication.Models
{
    public class EmployeeDetails
    {
        public int Emp_Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public int Employee_Number { get; set; }
        public string Employee_Name { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
        public DateTime? Joining_Date { get; set; }
        public string Gender { get; set; }
        public string Monthly_Basic_Salary { get; set; }
        public string Monthly_HRA { get; set; }
        public string Monthly_TA { get; set; }
        public string Monthly_PF { get; set; }
        public string Monthly_ESI { get; set; }
        public string Monthly_Gross { get; set; }
        public string Monthly_Takehome { get; set; }
        public string Yearly_Gross { get; set; }
        public string Yearly_Takehome { get; set; }

        public List<AssetDetail> AssetDetails { get; set; } = new List<AssetDetail>();
    }
}