using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleProjectWebApplication.Models
{
    public class AssetDetail
    {
        public int Emp_Id { get; set; }
        public int Asset_Id { get; set; }
        public string Asset_Type { get; set; }
        public string Description { get; set; }
        public string Brand_Name { get; set; }
        public string Model_Name { get; set; }
        public string Assigned_To { get; set; }
        public bool IsDeleted { get; set; }

        public int RowIndex { get; set; }
    }
}