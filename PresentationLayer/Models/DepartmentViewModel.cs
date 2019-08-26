using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class DepartmentViewModel
    {
        public int DeptId { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string DepartmentHead { get; set; }
    }
}