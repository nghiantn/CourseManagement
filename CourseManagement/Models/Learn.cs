using System;
using System.Collections.Generic;

namespace CourseManagement.Models
{
    public partial class Learn
    {
        public int IdStudent { get; set; }
        public int IdCalendar { get; set; }
        public int IdLearn { get; set; }

        public virtual Calendar IdCalendarNavigation { get; set; }
        public virtual Account IdStudentNavigation { get; set; }
    }
}
