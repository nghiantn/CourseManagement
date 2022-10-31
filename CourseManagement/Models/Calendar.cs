using System;
using System.Collections.Generic;

namespace CourseManagement.Models
{
    public partial class Calendar
    {
        public Calendar()
        {
            Classes = new HashSet<Class>();
        }

        public int IdCalendar { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Length { get; set; }
        public int? IdCourse { get; set; }

        public virtual Course IdCourseNavigation { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
    }
}
