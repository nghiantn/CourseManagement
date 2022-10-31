using System;
using System.Collections.Generic;

namespace CourseManagement.Models
{
    public partial class Class
    {
        public Class()
        {
            Judges = new HashSet<Judge>();
        }

        public int IdClass { get; set; }
        public int? IdLecturer { get; set; }
        public int? IdStudent { get; set; }
        public int? IdCalendar { get; set; }

        public virtual Calendar IdCalendarNavigation { get; set; }
        public virtual Account IdLecturerNavigation { get; set; }
        public virtual Account IdStudentNavigation { get; set; }
        public virtual ICollection<Judge> Judges { get; set; }
    }
}
