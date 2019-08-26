using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Department { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string Credit { get; set; }
    }
}