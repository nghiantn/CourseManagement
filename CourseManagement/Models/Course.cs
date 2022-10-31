using System;
using System.Collections.Generic;

namespace CourseManagement.Models
{
    public partial class Course
    {
        public Course()
        {
            Calendars = new HashSet<Calendar>();
        }

        public int IdCourse { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? IdCategory { get; set; }

        public virtual Category IdCategoryNavigation { get; set; }
        public virtual ICollection<Calendar> Calendars { get; set; }
    }
}
