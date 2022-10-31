using System;
using System.Collections.Generic;

namespace CourseManagement.Models
{
    public partial class Category
    {
        public Category()
        {
            Courses = new HashSet<Course>();
        }

        public int IdCategory { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
