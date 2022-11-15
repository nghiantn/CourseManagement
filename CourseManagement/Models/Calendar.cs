using System;
using System.Collections.Generic;

namespace CourseManagement.Models
{
    public partial class Calendar
    {
        public Calendar()
        {
            Learns = new HashSet<Learn>();
        }

        public int IdCalendar { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? Length { get; set; }
        public int IdCourse { get; set; }
        public int IdTeacher { get; set; }
        public int Slotnow { get; set; }
        public int Slotmax { get; set; }
        public bool Active { get; set; }

        public virtual Course IdCourseNavigation { get; set; }
        public virtual Account IdTeacherNavigation { get; set; }
        public virtual ICollection<Learn> Learns { get; set; }
    }
}
